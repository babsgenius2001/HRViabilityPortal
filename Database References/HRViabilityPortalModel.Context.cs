﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HRViabilityPortal.Database_References
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class HRViabilityPortalEntities : DbContext
    {
        public HRViabilityPortalEntities()
            : base("name=HRViabilityPortalEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AuditLog> AuditLogs { get; set; }
        public virtual DbSet<BaiMuajjalDocumentsTable> BaiMuajjalDocumentsTables { get; set; }
        public virtual DbSet<BaiMuajjalFormsTable> BaiMuajjalFormsTables { get; set; }
        public virtual DbSet<BaiMuajjalGradesTable> BaiMuajjalGradesTables { get; set; }
        public virtual DbSet<BMDetail> BMDetails { get; set; }
        public virtual DbSet<BranchDocumentsTable> BranchDocumentsTables { get; set; }
        public virtual DbSet<BranchFormsTable> BranchFormsTables { get; set; }
        public virtual DbSet<BranchGradesTable> BranchGradesTables { get; set; }
        public virtual DbSet<EmailAccount> EmailAccounts { get; set; }
        public virtual DbSet<EmailBox> EmailBoxes { get; set; }
        public virtual DbSet<Facility> Facilities { get; set; }
        public virtual DbSet<FacilityDocument> FacilityDocuments { get; set; }
        public virtual DbSet<FacilityForm> FacilityForms { get; set; }
        public virtual DbSet<FacilityUndertaking> FacilityUndertakings { get; set; }
        public virtual DbSet<HomeFinanceDocumentsTable> HomeFinanceDocumentsTables { get; set; }
        public virtual DbSet<HomeFinanceFormsTable> HomeFinanceFormsTables { get; set; }
        public virtual DbSet<HomeFinanceGradesTable> HomeFinanceGradesTables { get; set; }
        public virtual DbSet<HRBranchesMaster> HRBranchesMasters { get; set; }
        public virtual DbSet<HRFacilityMaster> HRFacilityMasters { get; set; }
        public virtual DbSet<HRPayroll> HRPayrolls { get; set; }
        public virtual DbSet<IjaraServiceDocumentsTable> IjaraServiceDocumentsTables { get; set; }
        public virtual DbSet<IjaraServiceFormsTable> IjaraServiceFormsTables { get; set; }
        public virtual DbSet<IjaraServiceGradesTable> IjaraServiceGradesTables { get; set; }
        public virtual DbSet<MurabahaDocumentsTable> MurabahaDocumentsTables { get; set; }
        public virtual DbSet<MurabahaFormsTable> MurabahaFormsTables { get; set; }
        public virtual DbSet<MurabahaGradesTable> MurabahaGradesTables { get; set; }
        public virtual DbSet<ReqHistory> ReqHistories { get; set; }
        public virtual DbSet<StaffGradesSalary> StaffGradesSalaries { get; set; }
        public virtual DbSet<StaffGradesAllowance> StaffGradesAllowances { get; set; }
        public virtual DbSet<HomeFinance18PercentDocumentsTable> HomeFinance18PercentDocumentsTable { get; set; }
        public virtual DbSet<HomeFinance18PercentFormsTable> HomeFinance18PercentFormsTable { get; set; }
        public virtual DbSet<HomeFinance18PercentGradesTable> HomeFinance18PercentGradesTable { get; set; }
        public virtual DbSet<Murabaha18PercentDocumentsTable> Murabaha18PercentDocumentsTable { get; set; }
        public virtual DbSet<Murabaha18PercentFormsTable> Murabaha18PercentFormsTable { get; set; }
        public virtual DbSet<Murabaha18PercentGradesTable> Murabaha18PercentGradesTable { get; set; }
        public virtual DbSet<Murabaha5PercentDocumentsTable> Murabaha5PercentDocumentsTable { get; set; }
        public virtual DbSet<Murabaha5PercentFormsTable> Murabaha5PercentFormsTable { get; set; }
        public virtual DbSet<Murabaha5PercentGradesTable> Murabaha5PercentGradesTable { get; set; }
    
        public virtual ObjectResult<SELECT_REPORT_TRANS_Result> SELECT_REPORT_TRANS(string requestReference)
        {
            var requestReferenceParameter = requestReference != null ?
                new ObjectParameter("requestReference", requestReference) :
                new ObjectParameter("requestReference", typeof(string));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<SELECT_REPORT_TRANS_Result>("SELECT_REPORT_TRANS", requestReferenceParameter);
        }
    }
}
