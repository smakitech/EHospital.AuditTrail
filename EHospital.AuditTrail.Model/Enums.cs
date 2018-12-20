namespace EHospital.AuditTrail.Model
{
    /// TODO: How client service will know about such <code>enum</code> to use.
    /// <summary>
    /// Constants set which specify action type for <see cref="ActionsLog"/> model.
    /// </summary>
    public enum ActionMode : byte
    {
        /// <summary>
        /// Action type - CRUD operation Create.
        /// </summary>
        Create = 1,

        /// <summary>
        /// Action type - CRUD operation Update.
        /// </summary>
        Update,

        /// <summary>
        /// Action type - CRUD operation Delete.
        /// </summary>
        Delete
    }
}