namespace DatabaseApplication.Interfaces
{
    public interface IInventoryService
    {
        long GetTotalStockIn(); // Gets total stock in
        long GetTotalStockCheckedOut();

        Dictionary<string, long> GetStockByCategory();
        
        // Placeholder for stock checked out, if needed
    }
}
