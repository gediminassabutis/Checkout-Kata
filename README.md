# Checkout-Kata

A small checkout kata (console app + libraries) that supports scanning SKUs, calculating totals and applying bulk offers (e.g. A: 3 for 130, B: 2 for 45).

## Prerequisites

- .NET 10 SDK

## Build and run

- To run the console app:
  - dotnet run --project Checkout-Kata/Checkout-Kata.csproj

The console app supports the following commands:
- `scan <SKU> <count>` — scan/add `<count>` units of product with SKU
- `total` — calculate and display basket total (applies offers)
- `clear` — clear the basket
- `addProduct <SKU> <price>` — register a product at runtime
- `addOffer <SKU> <count> <price>` — add a bulk offer at runtime
- `exit` — exit the app

## Running tests

- Run all tests:
  - dotnet test

- Or run the checkout logic tests only:
  - dotnet test ./Checkout/CheckoutLogic.Tests

## Notes

- Projects target .NET 10.
- Tests use mocked repositories and verify scanning, totals and offer application.