﻿

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
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


public partial class CostcoDevStatusEntities : DbContext
{
    public CostcoDevStatusEntities()
        : base("name=CostcoDevStatusEntities")
    {

    }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        throw new UnintentionalCodeFirstException();
    }


    public virtual DbSet<Phase> Phases { get; set; }

    public virtual DbSet<UserRole> UserRoles { get; set; }

    public virtual DbSet<Vertical> Verticals { get; set; }

    public virtual DbSet<AllowedUser> AllowedUsers { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<ProjectPhase> ProjectPhases { get; set; }

    public virtual DbSet<StatusUpdate> StatusUpdates { get; set; }

}

}

