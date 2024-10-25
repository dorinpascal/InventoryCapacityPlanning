﻿using System.ComponentModel.DataAnnotations;

namespace LEGO.Inventory.Capacity.Planning.Dtos.SalesOrder;

public class SalesOrderRequestDto
{
    [Required(ErrorMessage = "Finished Goods Name is required")]
    public string FinishedGoodsName { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than 0")]
    public int Quantity { get; set; }

    [Required(ErrorMessage = "Local Distribution Center Name is required")]
    public string LocalDistributionCenterName { get; set; } = string.Empty;
}