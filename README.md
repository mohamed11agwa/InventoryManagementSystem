# Fulfillment & Inventory Management Platform

A production-oriented ASP.NET Core Web API for managing products, categories, warehouses, inventory, and customer orders.

The platform provides a centralized operational system for maintaining product data, tracking stock across multiple warehouses, processing customer orders, enforcing role-based access control, and preserving important business history.

The solution is designed with a focus on **business rule enforcement, data consistency, traceability, maintainability, and safe concurrent operations**.
---
## key Highlights

• Implemented JWT authentication and role-based authorization for Administrator, Manager, Sales Agent, and Warehouse Operator
• Developed product, category, customer, warehouse, inventory, and order management workflows with full CRUD and lifecycle operations
• Implemented multi-warehouse inventory tracking with stock adjustments and detailed historical stock traceability
• Built transactional order processing with stock consumption/restoration, order status history, and business-rule-driven state transitions
• Implemented idempotent order creation with unique idempotency keys and race-condition handling to prevent duplicate orders
• Added optimistic concurrency protection using SQL Server row-versioning and transactional processing for critical inventory/order operations
• Applied CQRS with MediatR, FluentValidation, DTOs, mapping, and pipeline validation behaviors
• Implemented domain events, centralized result/error handling, global exception handling, and auditing
---

## Overview

The Fulfillment & Inventory Management Platform was built to replace disconnected inventory and order-processing workflows with a centralized API.

The system supports the operational lifecycle from maintaining products and warehouses to managing inventory and processing customer orders.

### Core Capabilities

- Product and category management
- Warehouse management
- Multi-warehouse inventory tracking
- Stock adjustments with historical traceability
- Customer management
- Customer order creation and processing
- Order lifecycle management
- Order cancellation
- Stock consumption and restoration
- Concurrent inventory protection
- Idempotent order creation
- Order status history
- Role-based authorization
- JWT authentication
- Auditing
- Centralized error handling
- Pagination and query filtering
- Automated validation
- Domain events
- Database migrations

---

## Business Context

The company sells physical products and stores inventory across multiple warehouses.

Previously, product information, warehouse stock, and order processing were handled through disconnected tools and manual checks, creating problems such as:

- Inaccurate inventory information
- Duplicate work
- Unclear responsibility
- Invalid inventory operations
- Difficulty tracing stock changes
- Risk of inconsistent order processing

This platform provides a single internal system for maintaining products, warehouses, inventory, and customer orders.

---

## Users & Responsibilities

The system defines four primary roles:

| Role | Responsibilities |
|---|---|
| **Administrator** | Manage users, access, products, categories, warehouses, and system-level operations |
| **Sales Agent** | Create and manage customer orders |
| **Warehouse Operator** | Perform stock-related and order-fulfillment operations |
| **Manager** | Review products, inventory, orders, activity, and operational information |

Authorization is enforced at the API/application level and is not dependent on simply hiding functionality from the user interface.

---
## Test Users

| Role | Email | Password |
|---|---|---|
| Administrator | admin@inventory.local | Password123! |
| Warehouse Operator | operator@inventory.local | Password123! |
| Manager | manager@inventory.local | Password123! |
| Sales Agent | sales@inventory.local | Password123! |

---
# Architecture

The solution follows **Clean Architecture** principles with clear separation between business rules, application use cases, infrastructure concerns, and API delivery.

```text
InventoryManagementSystem
│
├── src
│   │
│   ├── Inventory.Domain
│   │   ├── Categories
│   │   ├── Customers
│   │   ├── Identity
│   │   ├── Inventory
│   │   ├── Orders
│   │   ├── Products
│   │   ├── Warehouses
│   │   └── Common
│   │
│   ├── Inventory.Application
│   │   ├── Common
│   │   └── Features
│   │       ├── Categories
│   │       ├── Customers
│   │       ├── Identity
│   │       ├── Inventory
│   │       ├── Orders
│   │       ├── Products
│   │       └── Warehouses
│   │
│   ├── Inventory.Infrastructure
│   │   ├── Data
│   │   ├── Identity
│   │   └── Migrations
│   │
│   └── Inventory.Api
│       ├── Controllers
│       ├── Extensions
│       └── Program.cs
│
└── InventoryManagementSystem.slnx

