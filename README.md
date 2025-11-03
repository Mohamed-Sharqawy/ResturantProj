<?xml version="1.0" encoding="UTF-8"?>
<restaurant_project>
    <business_information>
        <restaurant_name>The Cozy Corner</restaurant_name>
        <description>A restaurant management system with flexible ordering options</description>
    </business_information>
    
    <business_model>
        <policy id="order_cancellation">
            <title>Order Cancellation Policy</title>
            <description>
                There is no order cancellation feature available in the system.
            </description>
            <rationale>
                The menu remains available 24/7 for customer convenience, and orders are processed immediately upon confirmation.
            </rationale>
        </policy>
        
        <policy id="customer_data_restoration">
            <title>Customer Data Restoration</title>
            <description>
                The system does not implement customer data restoration functionality.
            </description>
            <rationale>
                Customer integrity is not questioned as customers will be physically present to receive their orders in both Dine-in and Takeout scenarios.
            </rationale>
            <applicable_order_types>
                <order_type>Dine-in</order_type>
                <order_type>Takeout</order_type>
            </applicable_order_types>
        </policy>
        
        <policy id="delivery_orders">
            <title>Delivery Order Management</title>
            <description>
                For delivery orders, the system collects and stores essential customer information.
            </description>
            <required_information>
                <field name="customer_name" required="true">Full name of the customer</field>
                <field name="phone_number" required="true">Contact phone number</field>
                <field name="full_address" required="true">Complete delivery address</field>
            </required_information>
            <database_entity>
                <entity_name>DeliveryAddress</entity_name>
                <description>Stores all delivery-related customer information</description>
            </database_entity>
        </policy>
    </business_model>
    
    <order_types>
        <order_type name="Dine-in">
            <description>Customer orders and eats at the restaurant</description>
            <customer_presence>Required</customer_presence>
        </order_type>
        <order_type name="Takeout">
            <description>Customer orders and picks up from the restaurant</description>
            <customer_presence>Required</customer_presence>
        </order_type>
        <order_type name="Delivery">
            <description>Order is delivered to customer's address</description>
            <customer_presence>Not Required</customer_presence>
            <additional_data>DeliveryAddress entity</additional_data>
        </order_type>
    </order_types>
</restaurant_project>
