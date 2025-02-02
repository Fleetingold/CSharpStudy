﻿namespace Asp.Net_Core_WebApi.Models
{
    record PurchInWriteBackInfoParam(
        Guid ID, 
        string OrderNo, 
        Guid StoreID, 
        string StoreNum, 
        string StoreType, 
        string StoreArea,
        string StoreLocation,
        string DrugNum,
        string BatchNo,
        string ProductDate,
        string DueDate,
        decimal ReceiptQuantity,
        decimal ReceiptRejectQuantity,
        string Receipter,
        DateTime ReceiptTime,
        string ReceiptRemark,
        decimal CheckQuantity,
        decimal CheckRejectQuantity,
        string Checker,
        DateTime CheckTime,
        string CheckRemark,
        string CheckerND,
        decimal KeepQuantity,
        decimal KeepRejectQuantity,
        string Keeper,
        DateTime KeepTime,
        string KeepRemark,
        int ErpSn);
}
