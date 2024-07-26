# PhoneTopUpSystem.API

## Prerequisites

- Ensure that the `BalanceService` API is running, as `PhoneTopUpSystem.API` relies on it to check user balances.
- If you need to change the port for the `BalanceService`, update the port number in `BalanceService.cs` within the `PhoneTopUpSystem.API` project.

## Database Setup

Both projects use separate databases. Since there is no API available for creating or updating users in either project, manually create users in both databases. Ensure that the `id` in the user table of `PhoneTopUpSystem` matches the `topupUserId` in the `BalanceService`.
