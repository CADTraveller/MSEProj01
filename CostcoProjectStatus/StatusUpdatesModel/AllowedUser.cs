//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace StatusUpdatesModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class AllowedUser
    {
        public System.Guid UserID { get; set; }
        public string Email { get; set; }
        public int RoleID { get; set; }
    
        public virtual UserRole UserRole { get; set; }
    }
}
