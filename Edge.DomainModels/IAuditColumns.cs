namespace Edge.DomainModels
{
    public interface IAuditColumns
    {
        DateTime DateCreated { get; set; }
        DateTime DateModified { get; set; }
        string CreatedBy { get; set; }
        string ModifiedBy {  get; set; }
    }
}
