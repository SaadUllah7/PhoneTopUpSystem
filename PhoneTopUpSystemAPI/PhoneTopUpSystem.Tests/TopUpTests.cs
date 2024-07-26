using Microsoft.EntityFrameworkCore;
using PhoneTopUpSystem.API;
using PhoneTopUpSystem.Tests;

namespace TopUpServiceTests
{
    [TestClass]
    public class TopUpControllerTests
    {
        private IBalanceService _balanceServiceMock;
        private IBeneficiaryService _beneficiaryServiceMock;
        private ITopUpService _topUpServiceMock;
        private TopUpDbContext _dbContext;
        private TopUpController _topUpController;
        private BeneficiaryController _beneficiaryController;

        [TestInitialize]
        public void Initialize()
        {
            var options = new DbContextOptionsBuilder<TopUpDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _dbContext = new TopUpDbContext(options);

            _balanceServiceMock = new BalanceServiceMock();
            _topUpServiceMock = new TopUpService(_dbContext, _balanceServiceMock);
            _beneficiaryServiceMock = new BeneficiaryService(_dbContext);

            _topUpController = new TopUpController(_topUpServiceMock, _balanceServiceMock);
            _beneficiaryController = new BeneficiaryController(_beneficiaryServiceMock, _balanceServiceMock);
        }

        [TestMethod]
        public async Task Beneficiary_AddBeneficiary_ShouldAddBeneficiary()
        {
            var user = new User { Id = 1, UserName = "testuser", IsVerified = true };
            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();

            var result = await _beneficiaryController.AddBeneficiary(user.Id, "testc3");

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var beneficiary = okResult.Value as BeneficiaryDTO;
            Assert.IsNotNull(beneficiary);
            Assert.AreEqual("testc3", beneficiary.Nickname);
            Assert.AreEqual(user.Id, beneficiary.UserId);
        }

        [TestMethod]
        public async Task Beneficiary_GetBeneficiaries_ShouldReturnBeneficiaries()
        {
            var user = new User { Id = 1, UserName = "testuser", IsVerified = true };
            var beneficiaries = new List<Beneficiary>
            {
                new Beneficiary { Id = 1, UserId = 1, Nickname = "testc3" },
                new Beneficiary { Id = 2, UserId = 1, Nickname = "testc4" }
            };
            user.Beneficiaries = beneficiaries;

            _dbContext.Users.Add(user);

            _dbContext.Beneficiaries.AddRange(beneficiaries);
            _dbContext.SaveChanges();

            var result = await _beneficiaryController.GetBeneficiaries(user.Id);

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var returnedBeneficiaries = okResult.Value as List<BeneficiaryDTO>;
            Assert.IsNotNull(returnedBeneficiaries);
            Assert.AreEqual(2, returnedBeneficiaries.Count);
        }

        [TestMethod]
        public void TopUp_GetTopUpOptions_ShouldReturnTopUpOptions()
        {
            var result = _topUpController.GetTopUpOptions();

            Assert.IsInstanceOfType(result, typeof(OkObjectResult));
            var okResult = result as OkObjectResult;
            var options = okResult.Value as int[];
            Assert.IsNotNull(options);
            CollectionAssert.AreEqual(new[] { 5, 10, 20, 30, 50, 75, 100 }, options);
        }

        [TestMethod]
        public async Task TopUp_ShouldReturnBadRequest_WhenAmountExceedsMonthlyLimit()
        {
            var user = new User { Id = 1, UserName = "testuser", IsVerified = false };
            var beneficiary = new Beneficiary { Id = 1, UserId = 1, Nickname = "testc3" };
            user.Beneficiaries = new List<Beneficiary> { beneficiary };

            _dbContext.Users.Add(user);
            _dbContext.Beneficiaries.Add(beneficiary);
            _dbContext.SaveChanges();

            Exception exception = null;
            try
            {
                await _topUpController.TopUp(user.Id, beneficiary.Id, 1500);
            }
            catch (System.Exception ex)
            {
                exception = ex;
            }

            Assert.AreEqual("Monthly limit exceeded for this beneficiary.", exception?.Message);
        }

        [TestMethod]
        public async Task TopUp_ShouldReturnBadRequest_WhenBalanceIsInsufficient()
        {
            var user = new User { Id = 1, UserName = "testuser", IsVerified = true };
            var beneficiary = new Beneficiary { Id = 1, UserId = 1, Nickname = "testc3" };

            _dbContext.Users.Add(user);
            _dbContext.Beneficiaries.Add(beneficiary);
            _dbContext.SaveChanges();

            //debit balance to test the scenario
            await _balanceServiceMock.DebitBalanceAsync(user.Id, 800);

            Exception exception = null;
            try
            {
                await _topUpController.TopUp(user.Id, beneficiary.Id, 300);
            }
            catch (Exception ex)
            {
               exception = ex;
            }

            Assert.AreEqual("Insufficient balance.", exception?.Message);
        }


        [TestCleanup]
        public void Cleanup()
        {
            _dbContext.Database.EnsureDeleted();
            _dbContext.Dispose();
        }

    }
}

