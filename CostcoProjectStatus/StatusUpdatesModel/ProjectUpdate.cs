//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using Newtonsoft.Json;

namespace StatusUpdatesModel
{
    using System;
    using System.Collections.Generic;
    
    public partial class ProjectUpdate
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ProjectUpdate()
        {
            this.StatusUpdates = new HashSet<StatusUpdate>();
        }
    
        public System.Guid ProjectUpdateID { get; set; }
        public Nullable<System.Guid> ProjectID { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    

        public virtual Project Project { get; set; }


        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
    }
}
