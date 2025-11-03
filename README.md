# The Cozy Corner - Business Model Documentation

## Restaurant Information

**Restaurant Name:** The Cozy Corner  
**Description:** A restaurant management system with flexible ordering options

---

## Business Model & Policies

### 1. Order Cancellation Policy

**Policy:** There is no order cancellation feature available in the system.

**Rationale:**  
The menu remains available 24/7 for customer convenience, and orders are processed immediately upon confirmation.

---

### 2. Customer Data Restoration

**Policy:** The system does not implement customer data restoration functionality.

**Rationale:**  
Customer integrity is not questioned as customers will be physically present to receive their orders in both Dine-in and Takeout scenarios.

**Applicable Order Types:**
- Dine-in
- Takeout

---

### 3. Delivery Order Management

**Policy:** For delivery orders, the system collects and stores essential customer information.

**Required Information:**

| Field | Required | Description |
|-------|----------|-------------|
| Customer Name | ✓ | Full name of the customer |
| Phone Number | ✓ | Contact phone number |
| Full Address | ✓ | Complete delivery address |

**Database Entity:** `DeliveryAddress`  
Stores all delivery-related customer information.

---

## Order Types

### 🍽️ Dine-in
- **Description:** Customer orders and eats at the restaurant
- **Customer Presence:** Required

### 📦 Takeout
- **Description:** Customer orders and picks up from the restaurant
- **Customer Presence:** Required

### 🚚 Delivery
- **Description:** Order is delivered to customer's address
- **Customer Presence:** Not Required
- **Additional Data:** Uses DeliveryAddress entity

---

## Technical Implementation

### DeliveryAddress Entity
This database entity stores delivery-specific information including:
- Customer name
- Phone number
- Full delivery address

This entity is only utilized when the order type is set to "Delivery".
