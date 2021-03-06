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
    
    public partial class Project
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Project()
        {
            this.ProjectPhases = new HashSet<ProjectPhase>();
            this.ProjectUpdates = new HashSet<ProjectUpdate>();
            this.StatusUpdates = new HashSet<StatusUpdate>();
        }
    
        public System.Guid ProjectID { get; set; }
        public string ProjectName { get; set; }
        public string Description { get; set; }
        public Nullable<int> VerticalID { get; set; }
    
        public virtual Vertical Vertical { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectPhase> ProjectPhases { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProjectUpdate> ProjectUpdates { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<StatusUpdate> StatusUpdates { get; set; }
    }
}
