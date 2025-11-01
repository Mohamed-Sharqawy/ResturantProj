using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using ResturantProj.Models;
using ResturantProj.Models.Enums;
using ResturantProj.ResContext;
using ResturantProj.VMs;
using System;

namespace ResturantProj.Controllers
{
    public class OrderController : Controller
    {
        private readonly MyResContext db;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<OrderController> _logger;

        public OrderController(MyResContext context, IServiceScopeFactory scopeFactory, ILogger<OrderController> logger)
        {
            db = context;
            _scopeFactory = scopeFactory;
            _logger = logger;
        }

        [Authorize(Roles ="Admin")]
        public async Task<IActionResult> Index()
        {
            var orders = await db.Orders.ToListAsync();
            return View(orders);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var menuItems = db.MenuItems
                .Include(m => m.Category)
                .OrderBy(m => m.Id)
                .ToList();

            const int DailyLimit = 50;

            // Compute remaining availability for today
            foreach (var item in menuItems)
            {
                // How many left today
                int remaining = DailyLimit - item.DailyOrderCount;

                // Don't allow negative numbers
                item.DailyOrderCount = remaining > 0 ? remaining : 0;
            }

            ViewBag.MenuItems = menuItems;
            ViewBag.DailyLimit = DailyLimit;

            return View();
        }




        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Order order, int[] SelectedMenuItemIds, int[] Quantities)
        //{
        //    var menuItems = await db.MenuItems
        //        .Include(m => m.Category)
        //        .OrderBy(m => m.Id)
        //        .ToListAsync();
        //    ViewBag.MenuItems = menuItems;

        //    // --- Validate Delivery ---
        //    if (order.OrderType != OrderType.Delivery)
        //    {
        //        ModelState.Remove(nameof(order.DeliveryAddress));
        //    }
        //    else if (string.IsNullOrWhiteSpace(order.DeliveryAddress?.FullAddress))
        //    {
        //        ModelState.AddModelError(nameof(order.DeliveryAddress), "Address is required for delivery.");
        //    }

        //    // --- Validate selection ---
        //    var selectedIds = SelectedMenuItemIds ?? Array.Empty<int>();
        //    if (selectedIds.Length == 0)
        //    {
        //        ModelState.AddModelError("", "Please select at least one menu item.");
        //    }

        //    order.OrderItems = new List<OrderItem>();
        //    var selectedSet = new HashSet<int>(selectedIds);

        //    // --- Build order items ---
        //    for (int i = 0; i < menuItems.Count; i++)
        //    {
        //        var mi = menuItems[i];
        //        if (!selectedSet.Contains(mi.Id))
        //            continue;

        //        int qty = 1;
        //        if (Quantities != null && Quantities.Length == menuItems.Count)
        //        {
        //            qty = Quantities[i] > 0 ? Quantities[i] : 1;
        //        }
        //        else if (Quantities != null)
        //        {
        //            int pos = Array.IndexOf(selectedIds, mi.Id);
        //            if (pos >= 0 && pos < Quantities.Length)
        //                qty = Quantities[pos] > 0 ? Quantities[pos] : 1;
        //        }

        //        order.OrderItems.Add(new OrderItem
        //        {
        //            MenuItemId = mi.Id,
        //            Quantity = qty,
        //            UnitPrice = mi.Price,
        //            Subtotal = mi.Price * qty
        //        });
        //    }

        //    if (!order.OrderItems.Any())
        //    {
        //        ModelState.AddModelError("", "No valid items selected.");
        //    }

        //    // --- Compute totals ---
        //    order.SubTotal = order.OrderItems.Sum(x => x.Subtotal);
        //    order.DiscountAmount = 0m;
        //    order.TaxAmount = Math.Round(order.SubTotal * 0.08m, 2);
        //    order.Total = Math.Round(order.SubTotal + order.TaxAmount - order.DiscountAmount, 2);

        //    order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmss}";
        //    order.CreatedAt = DateTime.Now;
        //    order.Status = OrderStatus.Pending;
        //    order.StatusChangedAt = DateTime.Now;

        //    const int DailyLimit = 50;
        //    var today = DateTime.Today;
        //    var tomorrow = today.AddDays(1);
        //    var menuItemIdsInOrder = order.OrderItems.Select(oi => oi.MenuItemId).Distinct().ToList();

        //    // --- Get today's total ordered quantities ---
        //    var orderedCountsToday = await db.OrderItems
        //        .Where(oi => menuItemIdsInOrder.Contains(oi.MenuItemId)
        //                     && oi.Order.CreatedAt >= today
        //                     && oi.Order.CreatedAt < tomorrow)
        //        .GroupBy(oi => oi.MenuItemId)
        //        .Select(g => new { MenuItemId = g.Key, TotalOrdered = g.Sum(x => x.Quantity) })
        //        .ToListAsync();

        //    var orderedCountsMap = orderedCountsToday.ToDictionary(x => x.MenuItemId, x => x.TotalOrdered);

        //    // --- Check for exceeding the daily limit ---
        //    foreach (var oi in order.OrderItems)
        //    {
        //        orderedCountsMap.TryGetValue(oi.MenuItemId, out var alreadyOrderedToday);
        //        var totalIfAdded = alreadyOrderedToday + oi.Quantity;

        //        if (totalIfAdded > DailyLimit)
        //        {
        //            var mi = menuItems.FirstOrDefault(m => m.Id == oi.MenuItemId);
        //            var remaining = Math.Max(0, DailyLimit - alreadyOrderedToday);
        //            ModelState.AddModelError("",
        //                $"{mi?.Name ?? "Item"} exceeds the daily limit. Only {remaining} remaining for today.");
        //        }
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(order);
        //    }

        //    // --- Save order and update counts ---
        //    using (var transaction = await db.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            db.Orders.Add(order);
        //            await db.SaveChangesAsync();

        //            var idsToUpdate = order.OrderItems.Select(oi => oi.MenuItemId).Distinct().ToList();
        //            var itemsToUpdate = await db.MenuItems
        //                .Where(m => idsToUpdate.Contains(m.Id))
        //                .ToListAsync();

        //            foreach (var oi in order.OrderItems)
        //            {
        //                var menuItem = itemsToUpdate.First(m => m.Id == oi.MenuItemId);
        //                menuItem.DailyOrderCount += oi.Quantity;
        //                db.MenuItems.Update(menuItem);
        //            }

        //            await db.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //        }
        //        catch
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }

        //    TempData["SuccessMessage"] = "Order placed successfully!";
        //    return RedirectToAction("Index", "MenuItem");
        //}



        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> Create(Order order, int[] SelectedMenuItemIds, int[] Quantities)
        //{
        //    var menuItems = await db.MenuItems
        //        .Include(m => m.Category)
        //        .OrderBy(m => m.Id)
        //        .ToListAsync();
        //    ViewBag.MenuItems = menuItems;

        //    // --- Validate delivery ---
        //    if (order.OrderType != OrderType.Delivery)
        //    {
        //        ModelState.Remove(nameof(order.DeliveryAddress));
        //    }
        //    else if (string.IsNullOrWhiteSpace(order.DeliveryAddress?.FullAddress))
        //    {
        //        ModelState.AddModelError(nameof(order.DeliveryAddress), "Address is required for delivery.");
        //    }

        //    // --- Validate selection ---
        //    var selectedIds = SelectedMenuItemIds ?? Array.Empty<int>();
        //    if (selectedIds.Length == 0)
        //    {
        //        ModelState.AddModelError("", "Please select at least one menu item.");
        //    }

        //    order.OrderItems = new List<OrderItem>();
        //    var selectedSet = new HashSet<int>(selectedIds);

        //    // --- Build order items ---
        //    for (int i = 0; i < menuItems.Count; i++)
        //    {
        //        var mi = menuItems[i];
        //        if (!selectedSet.Contains(mi.Id))
        //            continue;

        //        int qty = 1;
        //        if (Quantities != null && Quantities.Length == menuItems.Count)
        //        {
        //            qty = Quantities[i] > 0 ? Quantities[i] : 1;
        //        }
        //        else if (Quantities != null)
        //        {
        //            int pos = Array.IndexOf(selectedIds, mi.Id);
        //            if (pos >= 0 && pos < Quantities.Length)
        //                qty = Quantities[pos] > 0 ? Quantities[pos] : 1;
        //        }

        //        order.OrderItems.Add(new OrderItem
        //        {
        //            MenuItemId = mi.Id,
        //            Quantity = qty,
        //            UnitPrice = mi.Price,
        //            Subtotal = mi.Price * qty
        //        });
        //    }

        //    if (!order.OrderItems.Any())
        //    {
        //        ModelState.AddModelError("", "No valid items selected.");
        //    }

        //    // --- Compute subtotal ---
        //    order.SubTotal = order.OrderItems.Sum(x => x.Subtotal);

        //    // --- Apply discounts ---
        //    var egyptZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
        //    var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);

        //    order.DiscountAmount = 0m;
        //    if (localNow.Hour >= 15 && localNow.Hour < 17)
        //        order.DiscountAmount += order.SubTotal * 0.20m; // Happy hour 20%
        //    if (order.SubTotal > 100)
        //        order.DiscountAmount += (order.SubTotal - order.DiscountAmount) * 0.10m; // Bulk 10%

        //    order.TaxAmount = Math.Round(order.SubTotal * 0.08m, 2);
        //    order.Total = Math.Round(order.SubTotal + order.TaxAmount - order.DiscountAmount, 2);

        //    // --- Set order meta ---
        //    order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmss}";
        //    order.CreatedAt = DateTime.UtcNow;
        //    order.Status = OrderStatus.Pending;
        //    order.StatusChangedAt = DateTime.UtcNow;

        //    const int DailyLimit = 50;
        //    var today = DateTime.Today;
        //    var tomorrow = today.AddDays(1);
        //    var menuItemIdsInOrder = order.OrderItems.Select(oi => oi.MenuItemId).Distinct().ToList();

        //    // --- Get today's total ordered quantities ---
        //    var orderedCountsToday = await db.OrderItems
        //        .Where(oi => menuItemIdsInOrder.Contains(oi.MenuItemId)
        //                     && oi.Order.CreatedAt >= today
        //                     && oi.Order.CreatedAt < tomorrow)
        //        .GroupBy(oi => oi.MenuItemId)
        //        .Select(g => new { MenuItemId = g.Key, TotalOrdered = g.Sum(x => x.Quantity) })
        //        .ToListAsync();

        //    var orderedCountsMap = orderedCountsToday.ToDictionary(x => x.MenuItemId, x => x.TotalOrdered);

        //    // --- Check for exceeding daily limit ---
        //    foreach (var oi in order.OrderItems)
        //    {
        //        orderedCountsMap.TryGetValue(oi.MenuItemId, out var alreadyOrderedToday);
        //        var totalIfAdded = alreadyOrderedToday + oi.Quantity;

        //        if (totalIfAdded > DailyLimit)
        //        {
        //            var mi = menuItems.FirstOrDefault(m => m.Id == oi.MenuItemId);
        //            var remaining = Math.Max(0, DailyLimit - alreadyOrderedToday);
        //            ModelState.AddModelError("",
        //                $"{mi?.Name ?? "Item"} exceeds the daily limit. Only {remaining} remaining for today.");
        //        }
        //    }

        //    if (!ModelState.IsValid)
        //    {
        //        return View(order);
        //    }

        //    // --- Save order ---
        //    using (var transaction = await db.Database.BeginTransactionAsync())
        //    {
        //        try
        //        {
        //            db.Orders.Add(order);
        //            await db.SaveChangesAsync();

        //            var idsToUpdate = order.OrderItems.Select(oi => oi.MenuItemId).Distinct().ToList();
        //            var itemsToUpdate = await db.MenuItems
        //                .Where(m => idsToUpdate.Contains(m.Id))
        //                .ToListAsync();

        //            foreach (var oi in order.OrderItems)
        //            {
        //                var menuItem = itemsToUpdate.First(m => m.Id == oi.MenuItemId);
        //                menuItem.DailyOrderCount += oi.Quantity;
        //                db.MenuItems.Update(menuItem);
        //            }

        //            await db.SaveChangesAsync();
        //            await transaction.CommitAsync();
        //        }
        //        catch
        //        {
        //            await transaction.RollbackAsync();
        //            throw;
        //        }
        //    }

        //    // --- Schedule order status changes ---
        //    _ = Task.Run(async () =>
        //    {
        //        try
        //        {
        //            // Wait 5 minutes => Preparing
        //            await Task.Delay(TimeSpan.FromMinutes(5));
        //            var freshOrder = await db.Orders.FindAsync(order.Id);
        //            if (freshOrder != null && freshOrder.Status == OrderStatus.Pending)
        //            {
        //                freshOrder.Status = OrderStatus.Preparing;
        //                freshOrder.StatusChangedAt = DateTime.UtcNow;
        //                await db.SaveChangesAsync();
        //            }

        //            // Wait max prep time => Ready
        //            var maxPrepTime = order.OrderItems.Max(i => i.MenuItem?.PreparationTimeMinutes ?? 10);
        //            await Task.Delay(TimeSpan.FromMinutes(maxPrepTime));
        //            freshOrder = await db.Orders.FindAsync(order.Id);
        //            if (freshOrder != null && freshOrder.Status == OrderStatus.Preparing)
        //            {
        //                freshOrder.Status = OrderStatus.Ready;
        //                freshOrder.StatusChangedAt = DateTime.UtcNow;
        //                await db.SaveChangesAsync();
        //            }

        //            // Wait 30 min => Delivered (if delivery)
        //            if (order.OrderType == OrderType.Delivery)
        //            {
        //                await Task.Delay(TimeSpan.FromMinutes(30));
        //                freshOrder = await db.Orders.FindAsync(order.Id);
        //                if (freshOrder != null && freshOrder.Status == OrderStatus.Ready)
        //                {
        //                    freshOrder.Status = OrderStatus.Delivered;
        //                    freshOrder.StatusChangedAt = DateTime.UtcNow;
        //                    await db.SaveChangesAsync();
        //                }
        //            }
        //        }
        //        catch
        //        {
        //            // Avoid crashing background task
        //        }
        //    });

        //    TempData["SuccessMessage"] = "Order placed successfully!";
        //    return RedirectToAction("Index", "MenuItem");
        //}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order, int[] SelectedMenuItemIds, int[] Quantities)
        {
            // Load menu items for view & processing
            var menuItems = await db.MenuItems
                .Include(m => m.Category)
                .OrderBy(m => m.Id)
                .ToListAsync();
            ViewBag.MenuItems = menuItems;
            const int DailyLimit = 50;
            ViewBag.DailyLimit = DailyLimit;

            // --- Basic delivery validation ---
            if (order.OrderType != OrderType.Delivery)
            {
                ModelState.Remove(nameof(order.DeliveryAddress));
            }
            else if (string.IsNullOrWhiteSpace(order.DeliveryAddress?.FullAddress))
            {
                ModelState.AddModelError(nameof(order.DeliveryAddress), "Address is required for delivery.");
                order.SubTotal += 12;
            }

            // --- Read selected ids & quantities ---
            var selectedIds = SelectedMenuItemIds ?? Array.Empty<int>();

            // Build a mapping SelectedMenuItemId -> Quantity (robust)
            var qtyMap = new Dictionary<int, int>();
            if (selectedIds.Length > 0 && Quantities != null && Quantities.Length > 0)
            {
                // case A: quantities posted aligned with selectedIds (common)
                if (Quantities.Length == selectedIds.Length)
                {
                    for (int i = 0; i < selectedIds.Length; i++)
                    {
                        qtyMap[selectedIds[i]] = Math.Max(1, Quantities[i]);
                    }
                }
                // case B: quantities posted for full menu (by menuItems index)
                else if (Quantities.Length == menuItems.Count)
                {
                    for (int i = 0; i < menuItems.Count; i++)
                    {
                        var id = menuItems[i].Id;
                        qtyMap[id] = Math.Max(1, Quantities[i]);
                    }
                }
                // fallback: attempt to map by index of id in selectedIds to quantities if lengths allow
                else
                {
                    for (int i = 0; i < selectedIds.Length && i < Quantities.Length; i++)
                        qtyMap[selectedIds[i]] = Math.Max(1, Quantities[i]);
                }
            }

            if (selectedIds.Length == 0)
            {
                ModelState.AddModelError("", "Please select at least one menu item.");
            }

            // --- Build order items using robust qty mapping ---
            order.OrderItems = new List<OrderItem>();
            var selectedSet = new HashSet<int>(selectedIds);

            foreach (var mi in menuItems)
            {
                if (!selectedSet.Contains(mi.Id))
                    continue;

                if (!mi.IsAvailable)
                {
                    ModelState.AddModelError("", $"{mi.Name} is currently unavailable.");
                    continue;
                }

                int qty = 1;
                if (qtyMap.TryGetValue(mi.Id, out var q))
                    qty = q;

                order.OrderItems.Add(new OrderItem
                {
                    MenuItemId = mi.Id,
                    Quantity = qty,
                    UnitPrice = mi.Price,
                    Subtotal = mi.Price * qty
                });
            }

            if (!order.OrderItems.Any())
            {
                ModelState.AddModelError("", "No valid items selected.");
            }

            // --- Compute totals ---
            order.SubTotal = order.OrderItems.Sum(x => x.Subtotal);

            // Discount logic (your existing)
            var egyptZone = TimeZoneInfo.FindSystemTimeZoneById("Egypt Standard Time");
            var localNow = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, egyptZone);

            order.DiscountAmount = 0m;
            if (localNow.Hour >= 15 && localNow.Hour < 17)
                order.DiscountAmount += order.SubTotal * 0.20m; // Happy hour 20%
            if (order.SubTotal > 100)
                order.DiscountAmount += (order.SubTotal - order.DiscountAmount) * 0.10m; // Bulk 10%

            order.TaxAmount = Math.Round(order.SubTotal * 0.08m, 2);
            order.Total = Math.Round(order.SubTotal + order.TaxAmount - order.DiscountAmount, 2);

            // --- Set order meta ---
            order.OrderNumber = $"ORD-{DateTime.Now:yyyyMMddHHmmss}";
            order.CreatedAt = DateTime.UtcNow;
            order.Status = OrderStatus.Pending;
            order.StatusChangedAt = DateTime.UtcNow;

            // --- Daily limit check (careful with UTC vs local) ---
            var today = DateTime.UtcNow.Date;            // use UTC date to match CreatedAt which is UTC
            var tomorrow = today.AddDays(1);

            var menuItemIdsInOrder = order.OrderItems.Select(oi => oi.MenuItemId).Distinct().ToList();

            var orderedCountsToday = await db.OrderItems
                .Where(oi => menuItemIdsInOrder.Contains(oi.MenuItemId)
                             && oi.Order.CreatedAt >= today
                             && oi.Order.CreatedAt < tomorrow)
                .GroupBy(oi => oi.MenuItemId)
                .Select(g => new { MenuItemId = g.Key, TotalOrdered = g.Sum(x => x.Quantity) })
                .ToListAsync();

            var orderedCountsMap = orderedCountsToday.ToDictionary(x => x.MenuItemId, x => x.TotalOrdered);

            foreach (var oi in order.OrderItems)
            {
                orderedCountsMap.TryGetValue(oi.MenuItemId, out var alreadyOrderedToday);
                var totalIfAdded = alreadyOrderedToday + oi.Quantity;
                if (totalIfAdded > DailyLimit)
                {
                    var mi = menuItems.FirstOrDefault(m => m.Id == oi.MenuItemId);
                    var remaining = Math.Max(0, DailyLimit - alreadyOrderedToday);
                    ModelState.AddModelError("", $"{mi?.Name ?? "Item"} exceeds the daily limit. Only {remaining} remaining for today.");
                }
            }

            // --- If invalid, log details and return the view (menu items must be present) ---
            if (!ModelState.IsValid)
            {
                // Log ModelState errors to help debugging
                _logger.LogWarning("Create(Order) ModelState invalid. SelectedIds: {SelectedIds}. QtyMap: {QtyMap}. Errors: {Errors}",
                    string.Join(",", selectedIds),
                    string.Join(", ", qtyMap.Select(kv => $"{kv.Key}:{kv.Value}")),
                    string.Join(" | ", ModelState.Where(kvp => kvp.Value.Errors.Any())
                                                 .Select(kvp => $"{kvp.Key}: {string.Join(",", kvp.Value.Errors.Select(e => e.ErrorMessage))}")));

                // Ensure the view has the menu items and daily limit
                ViewBag.MenuItems = menuItems;
                ViewBag.DailyLimit = DailyLimit;
                return View(order);
            }

            // --- Save in transaction and update MenuItem counts ---
            using (var transaction = await db.Database.BeginTransactionAsync())
            {
                try
                {
                    db.Orders.Add(order);
                    await db.SaveChangesAsync();

                    var idsToUpdate = order.OrderItems.Select(oi => oi.MenuItemId).Distinct().ToList();
                    var itemsToUpdate = await db.MenuItems
                        .Where(m => idsToUpdate.Contains(m.Id))
                        .ToListAsync();

                    foreach (var oi in order.OrderItems)
                    {
                        var menuItem = itemsToUpdate.First(m => m.Id == oi.MenuItemId);
                        menuItem.DailyOrderCount += oi.Quantity;

                        if (menuItem.DailyOrderCount >= DailyLimit)
                        {
                            menuItem.IsAvailable = false;
                        }

                        db.MenuItems.Update(menuItem);
                    }

                    await db.SaveChangesAsync();
                    await transaction.CommitAsync();
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError(ex, "Failed saving order in Create(Order). OrderNumber: {OrderNumber}", order.OrderNumber);
                    // rethrow or return error view — rethrow to keep behavior consistent with your previous code
                    throw;
                }
            }

            // --- Schedule status changes (keeps your scoped approach) ---
            _ = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(TimeSpan.FromMinutes(5));
                    using (var scope = _scopeFactory.CreateScope())
                    {
                        var scopedDb = scope.ServiceProvider.GetRequiredService<MyResContext>();
                        var freshOrder = await scopedDb.Orders.FindAsync(order.Id);
                        if (freshOrder != null && freshOrder.Status == OrderStatus.Pending)
                        {
                            freshOrder.Status = OrderStatus.Preparing;
                            freshOrder.StatusChangedAt = DateTime.UtcNow;
                            await scopedDb.SaveChangesAsync();
                        }

                        // compute max prep from the menu items we know
                        var maxPrepTime = 10;
                        try
                        {
                            // try read prep time from DB (safer than relying on detached order.OrderItems)
                            var ids = order.OrderItems.Select(x => x.MenuItemId).Distinct().ToList();
                            var items = await scopedDb.MenuItems.Where(m => ids.Contains(m.Id)).ToListAsync();
                            maxPrepTime = items.Max(m => m.PreparationTimeMinutes);
                        }
                        catch { /* fallback to 10 minutes */ }

                        await Task.Delay(TimeSpan.FromMinutes(maxPrepTime));

                        freshOrder = await scopedDb.Orders.FindAsync(order.Id);
                        if (freshOrder != null && freshOrder.Status == OrderStatus.Preparing)
                        {
                            freshOrder.Status = OrderStatus.Ready;
                            freshOrder.StatusChangedAt = DateTime.UtcNow;
                            await scopedDb.SaveChangesAsync();
                        }

                        if (order.OrderType == OrderType.Delivery)
                        {
                            await Task.Delay(TimeSpan.FromMinutes(30));
                            freshOrder = await scopedDb.Orders.FindAsync(order.Id);
                            if (freshOrder != null && freshOrder.Status == OrderStatus.Ready)
                            {
                                freshOrder.Status = OrderStatus.Delivered;
                                freshOrder.StatusChangedAt = DateTime.UtcNow;
                                await scopedDb.SaveChangesAsync();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Background order status updater failed for order id {OrderId}", order.Id);
                }
            });

            TempData["SuccessMessage"] = "Order placed successfully!";
            return RedirectToAction("Index", "Home");
        }
















    }


}
