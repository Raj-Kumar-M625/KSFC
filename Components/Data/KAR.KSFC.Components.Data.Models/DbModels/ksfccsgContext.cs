using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace KAR.KSFC.Components.Data.Models.DbModels
{
    public partial class ksfccsgContext : DbContext
    {
        public ksfccsgContext()
        {
        }

        public ksfccsgContext(DbContextOptions<ksfccsgContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CnstCdtab> CnstCdtabs { get; set; }
        public virtual DbSet<ConstmlaCdtab> ConstmlaCdtabs { get; set; }
        public virtual DbSet<ConstmpCdtab> ConstmpCdtabs { get; set; }
        public virtual DbSet<DistCdtab> DistCdtabs { get; set; }
        public virtual DbSet<TblEmprolehistDet> TblEmprolehistDets { get; set; }
        public virtual DbSet<DscDetailstab> DscDetailstabs { get; set; }
        public virtual DbSet<EmpsessionTab> EmpsessionTabs { get; set; }
        public virtual DbSet<HobCdtab> HobCdtabs { get; set; }
        public virtual DbSet<IpCdtab> IpCdtabs { get; set; }
        public virtual DbSet<KsfcuserDesignationDetail> KsfcuserDesignationDetails { get; set; }
        public virtual DbSet<KznCdtab> KznCdtabs { get; set; }
        public virtual DbSet<NuserhistoryTab> NuserhistoryTabs { get; set; }
        public virtual DbSet<OffcCdtab> OffcCdtabs { get; set; }
        public virtual DbSet<OtpTab> OtpTabs { get; set; }
        public virtual DbSet<PromUnitMaptab> PromUnitMaptabs { get; set; }
        public virtual DbSet<Promoter> Promoters { get; set; }
        public virtual DbSet<PromoterMaptab> PromoterMaptabs { get; set; }
        public virtual DbSet<PromsessionTab> PromsessionTabs { get; set; }
        public virtual DbSet<RegduserTab> RegduserTabs { get; set; }
        public virtual DbSet<TblAddressCdtab> TblAddressCdtabs { get; set; }
        public virtual DbSet<TblAssetcatCdtab> TblAssetcatCdtabs { get; set; }
        public virtual DbSet<TblAssettypeCdtab> TblAssettypeCdtabs { get; set; }
        public virtual DbSet<TblAttrCdtab> TblAttrCdtabs { get; set; }
        public virtual DbSet<TblAttrunitCdtab> TblAttrunitCdtabs { get; set; }
        public virtual DbSet<TblBankfacilityCdtab> TblBankfacilityCdtabs { get; set; }
        public virtual DbSet<TblBuildingCdtab> TblBuildingCdtabs { get; set; }
        public virtual DbSet<TblChairCdtab> TblChairCdtabs { get; set; }
        public virtual DbSet<TblChairmapCdtab> TblChairmapCdtabs { get; set; }
        public virtual DbSet<TblCnstCdtab> TblCnstCdtabs { get; set; }
        public virtual DbSet<TblCondCdtab> TblCondCdtabs { get; set; }
        public virtual DbSet<TblDoccatCdtab> TblDoccatCdtabs { get; set; }
        public virtual DbSet<TblDocdetailsCdtab> TblDocdetailsCdtabs { get; set; }
        public virtual DbSet<TblDomiCdtab> TblDomiCdtabs { get; set; }
        public virtual DbSet<TblDopCdtab> TblDopCdtabs { get; set; }
        public virtual DbSet<TblDophistDet> TblDophistDets { get; set; }
        public virtual DbSet<TblEgStatusMast> TblEgStatusMasts { get; set; }
        public virtual DbSet<TblEmpchairDet> TblEmpchairDets { get; set; }
        public virtual DbSet<TblEmpchairhistDet> TblEmpchairhistDets { get; set; }
        public virtual DbSet<TblEmpdesigTab> TblEmpdesigTabs { get; set; }
        public virtual DbSet<TblEmpdesighistTab> TblEmpdesighistTabs { get; set; }
        public virtual DbSet<TblEmpdscTab> TblEmpdscTabs { get; set; }
        public virtual DbSet<TblEmploginTab> TblEmploginTabs { get; set; }
        public virtual DbSet<TblEnqAddressDet> TblEnqAddressDets { get; set; }
        public virtual DbSet<TblEnqBankDet> TblEnqBankDets { get; set; }
        public virtual DbSet<TblEnqBasicDet> TblEnqBasicDets { get; set; }
        public virtual DbSet<TblEnqDocDet> TblEnqDocDets { get; set; }
        public virtual DbSet<TblEnqDocument> TblEnqDocuments { get; set; }
        public virtual DbSet<TblEnqGassetDet> TblEnqGassetDets { get; set; }
        public virtual DbSet<TblEnqGbankDet> TblEnqGbankDets { get; set; }
        public virtual DbSet<TblEnqGliabDet> TblEnqGliabDets { get; set; }
        public virtual DbSet<TblEnqGnwDet> TblEnqGnwDets { get; set; }
        public virtual DbSet<TblEnqGuarDet> TblEnqGuarDets { get; set; }
        public virtual DbSet<TblEnqMftotalDet> TblEnqMftotalDets { get; set; }
        public virtual DbSet<TblEnqPassetDet> TblEnqPassetDets { get; set; }
        public virtual DbSet<TblEnqPbankDet> TblEnqPbankDets { get; set; }
        public virtual DbSet<TblEnqPjcostDet> TblEnqPjcostDets { get; set; }
        public virtual DbSet<TblEnqPjfinDet> TblEnqPjfinDets { get; set; }
        public virtual DbSet<TblEnqPjmfDet> TblEnqPjmfDets { get; set; }
        public virtual DbSet<TblEnqPliabDet> TblEnqPliabDets { get; set; }
        public virtual DbSet<TblEnqPnwDet> TblEnqPnwDets { get; set; }
        public virtual DbSet<TblEnqPromDet> TblEnqPromDets { get; set; }
        public virtual DbSet<TblEnqRegnoDet> TblEnqRegnoDets { get; set; }
        public virtual DbSet<TblEnqSecDet> TblEnqSecDets { get; set; }
        public virtual DbSet<TblEnqSfinDet> TblEnqSfinDets { get; set; }
        public virtual DbSet<TblEnqSisDet> TblEnqSisDets { get; set; }
        public virtual DbSet<TblEnqTemptab> TblEnqTemptabs { get; set; }
        public virtual DbSet<TblEnqTrcostDet> TblEnqTrcostDets { get; set; }
        public virtual DbSet<TblEnqWcDet> TblEnqWcDets { get; set; }
        public virtual DbSet<TblFincompCdtab> TblFincompCdtabs { get; set; }
        public virtual DbSet<TblFinyearCdtab> TblFinyearCdtabs { get; set; }
        public virtual DbSet<TblIndCdtab> TblIndCdtabs { get; set; }
        public virtual DbSet<TblModuleCdtab> TblModuleCdtabs { get; set; }
        public virtual DbSet<TblNwCdtab> TblNwCdtabs { get; set; }
        public virtual DbSet<TblPclasCdtab> TblPclasCdtabs { get; set; }
        public virtual DbSet<TblPdesigCdtab> TblPdesigCdtabs { get; set; }
        public virtual DbSet<TblPjcostCdtab> TblPjcostCdtabs { get; set; }
        public virtual DbSet<TblPjcostgroupCdtab> TblPjcostgroupCdtabs { get; set; }
        public virtual DbSet<TblPjcsgroupCdtab> TblPjcsgroupCdtabs { get; set; }
        public virtual DbSet<TblPjmfCdtab> TblPjmfCdtabs { get; set; }
        public virtual DbSet<TblPjmfcatCdtab> TblPjmfcatCdtabs { get; set; }
        public virtual DbSet<TblPjsecCdtab> TblPjsecCdtabs { get; set; }
        public virtual DbSet<TblPremCdtab> TblPremCdtabs { get; set; }
        public virtual DbSet<TblProdCdtab> TblProdCdtab{ get; set; }
       
        public virtual DbSet<TblPromUid> TblPromUids { get; set; }
        public virtual DbSet<TblPromrelCdtab> TblPromrelCdtabs { get; set; }
        public virtual DbSet<TblPurpCdtab> TblPurpCdtabs { get; set; }
        public virtual DbSet<TblRegdetailsCdtab> TblRegdetailsCdtabs { get; set; }
        public virtual DbSet<TblRoleCdtab> TblRoleCdtabs { get; set; }
        public virtual DbSet<TblSecCdtab> TblSecCdtabs { get; set; }
        public virtual DbSet<TblSecProdMast> TblSecProdMasts { get; set; }
        public virtual DbSet<TblSizeCdtab> TblSizeCdtabs { get; set; }
        public virtual DbSet<TblSubattrCdtab> TblSubattrCdtabs { get; set; }
        public virtual DbSet<TblTrPjcostCdtab> TblTrPjcostCdtabs { get; set; }
        public virtual DbSet<TblTrgEmpGrade> TblTrgEmpGrades { get; set; }
        public virtual DbSet<TblTrgEmployee> TblTrgEmployees { get; set; }
        public virtual DbSet<TblUnitoptrCdtab> TblUnitoptrCdtabs { get; set; }
        public virtual DbSet<TlqCdtab> TlqCdtabs { get; set; }
        public virtual DbSet<UnitInfo1> UnitInfo1s { get; set; }
        public virtual DbSet<UnitMaptab> UnitMaptabs { get; set; }
        public virtual DbSet<VilCdtab> VilCdtabs { get; set; }
        public virtual DbSet<EnqAckDet> EnqAckDets { get; set; }
        public virtual DbSet<TblAppTeamDet> TblAppTeamDets { get; set; }
        public virtual DbSet<TblAppUnitAddress> TblAppUnitAddresses { get; set; }
        public virtual DbSet<TblAppUnitBank> TblAppUnitBanks { get; set; }
        public virtual DbSet<TblAppUnitDetail> TblAppUnitDetails { get; set; }
        public virtual DbSet<TblAppUnitLoanDet> TblAppUnitLoanDets { get; set; }
        public virtual DbSet<TblAppUnitProduct> TblAppUnitProducts { get; set; }
        public virtual DbSet<TblAppLoanMast> TblAppLoanMasts { get; set; } //By RV on 29-06-2022
        public virtual DbSet<TblIdmLegalWorkflow> TblIdmLegalWorkflows { get; set; } //By RV on 29-06-2022
        public virtual DbSet<TblUnitMast> TblUnitMasts { get; set; }  //By RV on 29-06-2022
        public virtual DbSet<TblIdmGuarDeedDet> TblIdmGuarDeedDet { get; set; }
        public virtual DbSet<TblAppGuarAssetDet> TblAppGuarAssetDet { get; set; }
        public virtual DbSet<TblSubRegistrarMast> TblSubRegistrarMasts { get; set; }
        public virtual DbSet<TblAppGuarLiabDet> TblAppGuarLiabDet { get; set; } //By MJ on 04-08-2022
        public virtual DbSet<TblAppGuarnator> TblAppGuarnator { get; set; }//By MJ on 04-08-2022
        public virtual DbSet<TblAppGuarNwDet> TblAppGuarNwDet { get; set; }//By MJ on 04-08-2022
        public virtual DbSet<TblChargeType> TblChargeType { get; set; }
        public virtual DbSet<TblIdmHypothDet> TblIdmHypothDet { get; set; }//By MJ on 09-08-2022
        public virtual DbSet<TblIdmHypothMap> TblIdmHypothMap  { get; set; }//By SR on 07-04-2023
        public virtual DbSet<TblAssetRefnoDet> TblAssetRefnoDet { get; set; }//By MJ on 09-08-2022
        public virtual DbSet<TblLdDocument> TblLdDocuments { get; set; }
        public virtual DbSet<TblUCDocument> TblUCDocuments { get; set; }
        
        public virtual DbSet<TblCUDocument> TblCUDocuments { get; set; }
        public virtual DbSet<TblIdmSidbiApproval> TblIdmSidbiApproval { get; set; } // By Dev on 19/08/2022
        public virtual DbSet<TblPromTypeCdtab> TblPromTypeCdtab { get; set; } //By Dev on 19/08/2022
        public virtual DbSet<TblIdmDsbFm813> TblIdmDsbFm813 { get; set; } // By MJ on 19/08/2022
        public virtual DbSet<TblAddlCondDet> TblAddlCondDet { get; set; }
        public virtual DbSet<TblCondStgMast> TblCondStgMast { get; set; }
        public virtual DbSet<TblIdmUnitDetails> TblIdmUnitDetails { get; set; }
        public virtual DbSet<TblIdmUnitAddress> TblIdmUnitAddress { get; set; }
        public virtual DbSet<TblIdmFirstInvestmentClause> TblIdmFirstInvestmentClause { get; set; } // By Akhila on 22/08/2022
        public virtual DbSet<Tblfm8fm13CDTab> Tblfm8fm13CDTab { get; set; }

        public virtual DbSet<IdmPromoter> IdmPromoter { get; set; } // By Dev on 29/08/2022
        public virtual DbSet<TbIIfscMaster> TbIIfscMaster { get; set; } // By Dev on 01/09/2022
        public virtual DbSet<IdmPromoterBankDetails> IdmPromoterBankDetails { get; set; } // By Dev on 01/09/2022
        public virtual DbSet<TblPsubclasCdtab> TblPsubclasCdtab { get; set; } // By Dev on 03/09/2022
        public virtual DbSet<TblPqualCdtab> TblPqualCdtab { get; set; } // By Dev on 03/09/2022
        public virtual DbSet<TblAcTypeCdtab> TblAcTypeCdtab { get; set; } // By Dev on 06/09/2022
        public virtual DbSet<TblPromterLiabDet> TblPromterLiabDet { get; set; } // By sandeep on 08/09/2022
        public virtual DbSet<TblIdmPromoterNetWork> TblIdmPromoterNetWork { get; set; } // By sandeep on 09/09/2022
        public virtual DbSet<IdmUnitProducts> IdmUnitProducts { get; set; }
        public virtual DbSet<IdmPromAssetDet> IdmPromAssetDet { get; set; }
        public virtual DbSet<TblIdmDspInsp> TblIdmDspInsp { get; set; }    // By Sandeep on 26/08/2022
        public virtual DbSet<TblIdmDchgBuildingDet> TblIdmDchgBuildingDet { get; set; }    // By Swetha on 25/08/2022     
        public virtual DbSet<TblIdmDchgWorkingCapital> TblIdmDchgWorkingCapital { get; set; }    // By Swetha on 30/08/2022
        public virtual DbSet<TblIdmDchgImportMachinery> TblIdmDchgImportMachinery { get; set; }    // By Swetha on 30/08/2022
        public virtual DbSet<TblIdmDchgMeansOfFinance> TblIdmDchgMeansOfFinance { get; set; }//By Swetha on 05/09/2022
        public virtual DbSet<TblStateZone> TblStateZone { get; set; }    // By Sandeep on 01/09/2022   
        public virtual DbSet<TblIdmDChgFurn> TblIdmDChgFurn { get; set; }    // By Sandeep on 30/08/2022
        public virtual DbSet<TblINSPDocument> TblINSPDocument { get; set; }    // By Sandeep on 30/08/2022


        //public virtual DbSet<TblIdmDchgLand> TblIdmDchgLandDet { get; set; } // By Manoj on 25/08/2022
        public virtual DbSet<TblIdmDchgProjectCost> TblIdmDchgProjectCost { get; set; }    // By Akhila on 05/09/2022

        public virtual DbSet<TblLaReceiptDet> TblLaReceiptDet { get; set; }

        public virtual DbSet<TblLaPaymentDet> TblLaPaidDet { get; set; }
        public virtual DbSet<TblLaReceiptPaymentDet> TblLaReceiptPaymentDet { get; set; }
        public virtual DbSet<CodeTable> CodeTable { get; set; }
        public virtual DbSet<TblIdmAuditDet> TblIdmAuditDet { get; set; } // By Dev on 3/10/2022


        public virtual DbSet<TblIdmIrLand> TblIdmIrLand { get; set; }
        public virtual DbSet<TblIdmIrPlmc> TblIdmIrPlmc { get; set; } // By Dev on 28/09/2022
        public virtual DbSet<TblLandTypeMast> TblLandTypeMast { get; set; } // By Dev on 01/10/2022

        public virtual DbSet<TblIdmBuildingAcquisitionDetails> TblIdmBuildingAcquisitionDetails { get; set; } // By Ram on 01/10/2022
        public virtual DbSet<TblIdmIrFurn> TblIdmIrFurn { get; set; }    //By Kiran Vasishta TS on 28-SEP-2022


        // Creation of Disbursement Proposal
        public virtual DbSet<TblIdmDisbProp> TblIdmDisbProp { get; set; }
        public virtual DbSet<TblIdmReleDetls> TblIdmReleDetls { get; set; }
        public virtual DbSet<TblIdmBenfDet> TblIdmBenfDet { get; set; } // By Dev on 07/10/2022
        public virtual DbSet<IdmDsbdets> TblIdmDsbdets  { get; set; } // By Ram on 06/10/2022
        public virtual DbSet<IdmOthdebitsDetails> IdmOthdebitsDetails { get; set; } // By GK on 27/10/2022
        public virtual DbSet<IdmOthdebitsMast> IdmOthdebitsMast { get; set; } // By GK on 27/10/2022
        public virtual DbSet<TblPincodeMaster> TblPincodeMaster { get; set; } // By GK on 16/11/2022
        public virtual DbSet<TblCondStageMast> TblCondStageMast { get; set; } // By GK on 19/12/2022
        
        public virtual DbSet<TblProcureMast> TblProcureMast { get; set; }
        public virtual DbSet<TblCurrencyMast> TblCurrencyMast { get; set; }
        public virtual DbSet<TblMachineryStatus> TblMachineryStatus { get; set; }
        public virtual DbSet<TblDsbStatImp> TblDsbStatImps { get; set; } // By SR on 24/04/2023
        public virtual DbSet<TblIdmProjland> TblIdmProjlands { get; set; } // By SR on 16/05/2023
        public virtual DbSet<TblIdmProjBldg> TblIdmProjBldgs { get; set; } // By SR on 16/05/2023
        public virtual DbSet<TblIdmProjImpMc> TblIdmProjImpMcs { get; set; } // By SR on 16/05/2023
        public virtual DbSet<TblIdmProjPlmc> TblIdmProjPlmcs { get; set; } // By SR on 16/05/2023
        public virtual DbSet<TblIdmFurn> TblIdmFurns { get; set; } // By SR on 16/05/2023


        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) 
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
               optionsBuilder.UseMySQL("server=127.0.0.1;port=3305;user=root;database=ksfc_oct;password=8147771609Raj@");
              // optionsBuilder.UseMySQL("server=ksfccsgdb.c3d9d6stfnzl.ap-south-1.rds.amazonaws.com;port=3306;user=KSFCCSGUSER;database=ksfc_oct;password=Anand123;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            //By RV on 29-06-2022
            modelBuilder.Entity<TblLdDocument>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_Ld_document");

                entity.Property(e => e.SubModuleId)
                    .HasColumnName("SubModuleId");

                entity.Property(e => e.SubModuleType)
                    .HasMaxLength(50)
                    .HasColumnName("SubModuleType");

                entity.Property(e => e.MainModule)
                  .HasMaxLength(50)
                  .HasColumnName("MainModule");

                entity.Property(e => e.FileName)
                    .HasMaxLength(150)
                    .HasColumnName("FileName");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(200)
                    .HasColumnName("FilePath");

                entity.Property(e => e.FileType)
                    .HasMaxLength(45)
                    .HasColumnName("FileType");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblUCDocument>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_Uc_document");

                entity.Property(e => e.SubModuleId)
                    .HasColumnName("SubModuleId");

                entity.Property(e => e.SubModuleType)
                    .HasMaxLength(50)
                    .HasColumnName("SubModuleType");

                entity.Property(e => e.MainModule)
                 .HasMaxLength(50)
                 .HasColumnName("MainModule");


                entity.Property(e => e.FileName)
                    .HasMaxLength(150)
                    .HasColumnName("FileName");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(200)
                    .HasColumnName("FilePath");

                entity.Property(e => e.FileType)
                    .HasMaxLength(45)
                    .HasColumnName("FileType");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblDCDocument>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dc_document");

                entity.Property(e => e.SubModuleId)
                    .HasColumnName("SubModuleId");

                entity.Property(e => e.SubModuleType)
                    .HasMaxLength(50)
                    .HasColumnName("SubModuleType");

                entity.Property(e => e.MainModule)
                 .HasMaxLength(50)
                 .HasColumnName("MainModule");


                entity.Property(e => e.FileName)
                    .HasMaxLength(150)
                    .HasColumnName("FileName");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(200)
                    .HasColumnName("FilePath");

                entity.Property(e => e.FileType)
                    .HasMaxLength(45)
                    .HasColumnName("FileType");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });


            modelBuilder.Entity<TblINSPDocument>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_insp_document");

                entity.Property(e => e.SubModuleId)
                    .HasColumnName("SubModuleId");

                entity.Property(e => e.SubModuleType)
                    .HasMaxLength(50)
                    .HasColumnName("SubModuleType");

                entity.Property(e => e.MainModule)
                 .HasMaxLength(50)
                 .HasColumnName("MainModule");


                entity.Property(e => e.FileName)
                    .HasMaxLength(150)
                    .HasColumnName("FileName");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(200)
                    .HasColumnName("FilePath");

                entity.Property(e => e.FileType)
                    .HasMaxLength(45)
                    .HasColumnName("FileType");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblCUDocument>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_Cu_document");

                entity.Property(e => e.SubModuleId)
                    .HasColumnName("SubModuleId");

                entity.Property(e => e.SubModuleType)
                    .HasMaxLength(50)
                    .HasColumnName("SubModuleType");

                entity.Property(e => e.MainModule)
                 .HasMaxLength(50)
                 .HasColumnName("MainModule");


                entity.Property(e => e.FileName)
                    .HasMaxLength(150)
                    .HasColumnName("FileName");

                entity.Property(e => e.FilePath)
                    .HasMaxLength(200)
                    .HasColumnName("FilePath");

                entity.Property(e => e.FileType)
                    .HasMaxLength(45)
                    .HasColumnName("FileType");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblSubRegistrarMast>(entity =>
            {
                entity.HasKey(e => e.SrOfficeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_subregistrar_mast");

                entity.HasIndex(e => e.DistCd, "fk_tbl_subregistrar_mast_dist_cdtab");
                entity.HasIndex(e => e.TlqCd, "fk_tbl_subregistrar_mast_tlq_cdtab");

                entity.Property(e => e.SrOfficeId)
                    .HasColumnName("sr_office_id");

                entity.Property(e => e.SubRegistrarCd)
                    .HasColumnName("subregistrar_cd");

                entity.Property(e => e.SrCode)
                    .HasMaxLength(100)
                    .HasColumnName("sr_code");

                entity.Property(e => e.SrOfficeName)
                    .HasMaxLength(200)
                    .HasColumnName("sr_office_name");

                entity.Property(e => e.DistCd)
                    .HasColumnName("dist_cd");

                entity.Property(e => e.TlqCd)
                    .HasColumnName("tlq_cd");

                entity.Property(e => e.SrOthDetails)
                    .HasMaxLength(500)
                    .HasColumnName("sr_oth_details");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblAppLoanMast>(entity =>
            {
                entity.HasKey(e => e.InMastId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_loan_mast");

                entity.HasIndex(e => e.InOffc, "FK_tbl_app_loan_mast_offc_cdtab");

                entity.HasIndex(e => e.InUnit, "fk_tbl_app_loan_mast_tbl_unit_mast");

                entity.HasIndex(e => e.InTy, "fk_tbl_app_loan_mast_tbl_loan_type_cdtab");

                entity.HasIndex(e => e.InSchm, "fk_tbl_app_loan_mast_tbl_scheme_cdtab");

                entity.Property(e => e.InMastId)
                    .HasColumnName("ln_mast_id");

                entity.Property(e => e.InOffc)
                    .HasColumnName("ln_offc");

                entity.Property(e => e.InUnit)
                    .HasColumnName("ln_unit");

                entity.Property(e => e.InSno)
                    .HasColumnName("ln_sno");

                entity.Property(e => e.InNo)
                    .HasColumnName("ln_no");

                entity.Property(e => e.InTy)
                    .HasColumnName("ln_ty");

                entity.Property(e => e.InSanAmt)
                    .HasColumnName("ln_san_amt");

                entity.Property(e => e.InSanDt)
                    .HasColumnName("ln_san_dt");

                entity.Property(e => e.InSchm)
                    .HasColumnName("ln_schm");

                entity.Property(e => e.InStat)
                    .HasColumnName("ln_stat");

                entity.Property(e => e.InIntrLow)
                    .HasColumnName("ln_intr_low");

                entity.Property(e => e.InIntrHigh)
                    .HasColumnName("ln_intr_high");

                entity.Property(e => e.InIntReb)
                    .HasColumnName("ln_int_reb");

                entity.Property(e => e.InPmode)
                    .HasColumnName("ln_pmode");

                entity.Property(e => e.InImode)
                    .HasColumnName("ln_imode");

                entity.Property(e => e.InMortPrd)
                    .HasColumnName("ln_mort_prd");

                entity.Property(e => e.UnitId)
                    .HasColumnName("unit_id");

                entity.Property(e => e.InPurTy)
                    .HasColumnName("ln_pur_ty");

                entity.Property(e => e.InSub)
                    .HasColumnName("ln_sub");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblIdmLegalWorkflow>(entity =>
            {
                entity.HasKey(e => e.IdmLegalWorkflowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_legal_workflow");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_legal_workflow_offc_cdtab");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_legal_workflow_tbl_app_loan_mast");

                entity.HasIndex(e => e.EmpIdFrom, "fk_tbl_idm_legal_workflow_tbl_idm_dsb_spl_cleg");

                entity.HasIndex(e => e.EmpIdTo, "fk_tbl_idm_legal_workflow_tbl_idm_dsb_spl_cleg_1");

                entity.Property(e => e.IdmLegalWorkflowId)
                    .HasColumnName("idm_legal_workflow_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                 .HasColumnName("offc_cd"); // by GS

                entity.Property(e => e.EmpIdFrom)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id_from");

                entity.Property(e => e.ReceivedDate)
                    .HasColumnName("received_date");

                entity.Property(e => e.SendDate)
                    .HasColumnName("send_date");

                entity.Property(e => e.EmpIdTo)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id_to");

                entity.Property(e => e.WorkFlowStatusID)
                    .HasColumnName("worflow_status_id");

                entity.Property(e => e.IdmWfNoting)
                    .HasMaxLength(500)
                    .HasColumnName("idm_wf_noting");

                entity.Property(e => e.IdmWfRemarks)
                    .HasMaxLength(500)
                    .HasColumnName("idm_wf_remarks");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                /// <summary>
                ///  Author: Gowtham S; Date:1/08/2022
                /// </summary>

                entity.HasOne(e => e.TblAppLoanMast)
                 .WithOne(b => b.TblIdmLegalWorkflow)
               .HasForeignKey<TblIdmLegalWorkflow>(b => b.LoanAcc);

                // by GS
                entity.HasOne(e => e.OffcCdtab)
                      .WithOne(b => b.TblIdmLegalWorkflow)
                      .HasForeignKey<TblIdmLegalWorkflow>(b => b.OffcCd);

                // entity.HasOne(e => e.TblAppLoanMast)    //by GS
                //  .WithOne(b => b.TblIdmLegalWorkflow)
                // .HasForeignKey<TblIdmLegalWorkflow>(b => b.LoanSub);  //loansub is not defined as forign key

                //entity.HasOne(d => d.TgesCodeNavigation)
                //    .WithMany(p => p.TblEmpchairDets)
                //    .HasForeignKey(d => d.TgesCode)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("fk_tbl_empchair_det_tbl_trg_emp_grade");
            });

            modelBuilder.Entity<TblUnitMast>(entity =>
            {
                entity.HasKey(e => e.UtRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_unit_mast");

                entity.HasIndex(e => e.UtOffCd, "fk_tbl_unit_mast_offc_cdtab");

                entity.Property(e => e.UtRowId)
                    .HasColumnName("ut_rowid");

                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.UtName)
                    .HasMaxLength(100)
                    .HasColumnName("ut_name");

                entity.Property(e => e.UtOffCd)
                    .HasColumnName("ut_off_cd");

                entity.Property(e => e.UtUtPan)
                    .HasMaxLength(10)
                    .HasColumnName("ut_ut_pan");

                entity.Property(e => e.utFromDate)
                    .HasMaxLength(200)
                    .HasColumnName("ut_from_date");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.HasOne(e => e.TblAppLoanMast)
                       .WithOne(b => b.TblUnitMast)
                       .HasForeignKey<TblAppLoanMast>(b => b.InUnit);
            });

            /// <summary>
            ///  Author: Rajesh; Module: Primary Security; Date:27/07/2022
            /// </summary>
            modelBuilder.Entity<TblIdmDeedDet>(entity =>
            {
                entity.HasKey(e => e.IdmDeedDetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_deed_det");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_deed_det_offc_cdtab");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_deed_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.SecurityCd, "fk_tbl_idm_deed_det_tbl_security_refno_mast");

                entity.HasIndex(e => e.SubregistrarCd, "fk_tbl_idm_deed_det_tbl_subregistrar_mast");

                entity.HasIndex(e => e.ApprovedEmpId, "fk_tbl_idm_deed_det_tbl_trg_employee");

                entity.Property(e => e.IdmDeedDetId)
                    .HasColumnName("idm_deed_detid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.SecurityCd)
                    .HasColumnName("security_cd");

                entity.Property(e => e.DeedNo)
                    .HasMaxLength(20)
                    .HasColumnName("deed_no");

                entity.Property(e => e.DeedDesc)
                    .HasMaxLength(200)
                    .HasColumnName("deed_desc");

                entity.Property(e => e.SecurityValue)
                    .HasColumnName("security_value");

                entity.Property(e => e.SubregistrarCd)
                    .HasColumnName("subregistrar_cd");

                entity.Property(e => e.ExecutionDate)
                    .HasColumnName("execution_date");

                entity.Property(e => e.DeedUpload)
                    .HasMaxLength(300)
                    .HasColumnName("deed_upload");

                entity.Property(e => e.ApprovedEmpId)
                    .HasMaxLength(8)
                    .HasColumnName("approved_emp_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");


                entity.Property(e => e.PjSecNam)
                     .HasMaxLength(100)
                     .HasColumnName("pjsec_nam");

                entity.Property(e => e.PjSecDets)
                    .HasMaxLength(100)
                    .HasColumnName("pjsec_dets");

                //    entity.Property(e => e.PjsecDetsCd)
                //   .HasColumnName("pjsec_dets_cd");

                // entity.Property(e => e.PjSecRel)
                // .HasColumnName("pjsec_rel");

                // entity.Property(e => e.UtSlno)
                //.HasColumnName("ut_slno");


                entity.HasOne(e => e.TblSecurityRefnoMast)
                     .WithMany()
                     .HasForeignKey(c=>c.SecurityCd)
                     .HasConstraintName("fk_tbl_idm_deed_det_tbl_security_refno_mast");

            });

            modelBuilder.Entity<TblSecurityRefnoMast>(entity =>
            {
                entity.HasKey(e => e.SecRefnoMastId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_security_refno_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_security_refno_mast_offc_cdtab");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_security_refno_mast_tbl_app_loan_mast");

                entity.HasIndex(e => e.SecCd, "fk_tbl_security_refno_mast_tbl_sec_cdtab");

                entity.HasIndex(e => e.PjsecCd, "fk_tbl_security_refno_mast_tbl_pjsec_cdtab");

                entity.Property(e => e.SecRefnoMastId)
                    .HasColumnName("sec_refno_mast_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.SecurityCd)
                    .HasColumnName("security_cd");

                entity.Property(e => e.SecCd)
                    .HasColumnName("sec_cd");

                entity.Property(e => e.PjsecCd)
                    .HasColumnName("pjsec_cd");

                entity.Property(e => e.SecurityValue)
                    .HasColumnName("security_value");

                entity.Property(e => e.SecurityDetails)
                    .HasMaxLength(200)
                    .HasColumnName("security_details");

                entity.Property(e => e.SecNameHolder)
                    .HasMaxLength(200)
                    .HasColumnName("sec_name_holder");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.WhCharge)
                  .HasDefaultValue(true)
                  .HasColumnName("wh_charge");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.HasOne(e => e.TblIdmDeedDet)
                       .WithOne(b => b.TblSecurityRefnoMast)
                       .HasForeignKey<TblIdmDeedDet>(b => b.SecurityCd);

                entity.HasOne(e => e.TblSecCdtab)
                     .WithOne(b => b.TblSecurityRefnoMast)
                     .HasForeignKey<TblSecurityRefnoMast>(b => b.SecCd);

                entity.HasOne(e => e.TblPjsecCdtab)
                     .WithOne(b => b.TblSecurityRefnoMast)
                     .HasForeignKey<TblSecurityRefnoMast>(b => b.PjsecCd);

            });



            modelBuilder.Entity<TbIIfscMaster>(entity =>
            {
                entity.HasKey(e => e.IFSCRowID)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_ifsc_master");

                entity.Property(e => e.IFSCRowID)
                   .HasColumnName("ifsc_rowid");

                entity.Property(e => e.IFSCCode)
                    .HasMaxLength(11)
                    .HasColumnName("ifsc_cd");

                entity.Property(e => e.BankName)
                .HasMaxLength(100)
                    .HasColumnName("bank_name");

                entity.Property(e => e.BranchName)
                    .HasColumnName("branch_name");

                entity.Property(e => e.BankAddress)
                    .HasColumnName("bank_address");

                entity.Property(e => e.BankState)
                    .HasColumnName("bank_state");

                entity.Property(e => e.BankDistrict)
                    .HasColumnName("bank_district");

                entity.Property(e => e.BankTaluk)
                    .HasColumnName("bank_taluka");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                      .HasDefaultValue(false)
                    .HasMaxLength(200)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(200)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(200)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                //entity.HasOne(e => e.TblIdmDsbCharge)
                //       .WithOne(b => b.TbIIfscMaster)
                //       .HasForeignKey<TblIdmDsbCharge>(b => b.BankIfscCd);

            });




            modelBuilder.Entity<CnstCdtab>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("cnst_cdtab");

                entity.HasComment("Constitution Details");

                entity.Property(e => e.CnstCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("CNST_CD")
                    .HasComment("Constitution Code");

                entity.Property(e => e.CnstDets)
                    .HasMaxLength(30)
                    .HasColumnName("CNST_DETS")
                    .HasComment("Constitution Description");

                entity.Property(e => e.CnstPanchar)
                    .HasMaxLength(1)
                    .HasColumnName("CNST_PANCHAR")
                    .HasComment("4th Character of PAN");

                entity.Property(e => e.CnstTaxr)
                    .HasColumnType("decimal(5,2)")
                    .HasColumnName("CNST_TAXR")
                    .HasComment("Tax Rate");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<ConstmlaCdtab>(entity =>
            {
                entity.HasKey(e => e.ConstmlaCd)
                    .HasName("PRIMARY");

                entity.ToTable("constmla_cdtab");

                entity.HasComment("MLA Constituency Details");

                entity.Property(e => e.ConstmlaCd)
                    .HasColumnName("CONSTMLA_CD")
                    .HasComment("MLA constituency Code");

                entity.Property(e => e.ConstmlaKannada)
                    .HasMaxLength(100)
                    .HasColumnName("CONSTMLA_KANNADA")
                    .HasComment("Name of MLA Constituency in Kannada");

                entity.Property(e => e.ConstmlaName)
                    .HasMaxLength(50)
                    .HasColumnName("CONSTMLA_NAME")
                    .HasComment("Name of MLA Constituency");

                entity.Property(e => e.ConstmlaStateCd)
                    .HasMaxLength(50)
                    .HasColumnName("CONSTMLA_STATE_CD")
                    .HasComment("State Code of MLA Constituency");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<ConstmpCdtab>(entity =>
            {
                entity.HasKey(e => e.ConstmpCd)
                    .HasName("PRIMARY");

                entity.ToTable("constmp_cdtab");

                entity.HasComment("MP Constituency Details");

                entity.Property(e => e.ConstmpCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("CONSTMP_CD")
                    .HasComment("MP constituency Code");

                entity.Property(e => e.ConstmpKannada)
                    .HasMaxLength(100)
                    .HasColumnName("CONSTMP_KANNADA")
                    .HasComment("Name of MP Constituency in Kannada");

                entity.Property(e => e.ConstmpName)
                    .HasMaxLength(50)
                    .HasColumnName("CONSTMP_NAME")
                    .HasComment("Name of MP Constituency");

                entity.Property(e => e.ConstmpStateCd)
                    .HasMaxLength(50)
                    .HasColumnName("CONSTMP_STATE_CD")
                    .HasComment("State Code of MP Constituency");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<DistCdtab>(entity =>
            {
                entity.HasKey(e => e.DistCd)
                    .HasName("PRIMARY");

                entity.ToTable("dist_cdtab");

                entity.HasComment("District Details");

                entity.HasIndex(e => e.DistCircle, "fk_DIST_CDTAB_KZN_CDTAB");

                entity.Property(e => e.DistCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("DIST_CD")
                    .HasComment("District Code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DistBhoomicode).HasColumnName("DIST_BHOOMICODE");

                entity.Property(e => e.DistCircle)
                    .HasColumnType("tinyint")
                    .HasColumnName("DIST_CIRCLE")
                    .HasComment("Circle Code Ref: kzn_cdtab (kzn_cd)");

                entity.Property(e => e.DistFbFlg)
                    .HasMaxLength(1)
                    .HasColumnName("DIST_FB_FLG")
                    .HasComment("F=Forward, B=Backward");

                entity.Property(e => e.DistLgdcode).HasColumnName("DIST_LGDCODE");

                entity.Property(e => e.DistNam)
                    .HasMaxLength(250)
                    .HasColumnName("DIST_NAM")
                    .HasComment("District Name");

                entity.Property(e => e.DistNameKannada)
                    .HasMaxLength(100)
                    .HasColumnName("DIST_NAME_KANNADA");

                entity.Property(e => e.DistOffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("DIST_OFFC_CD")
                    .HasComment("Branch Code");

                entity.Property(e => e.DistZone)
                    .HasMaxLength(2)
                    .HasColumnName("DIST_ZONE")
                    .HasComment("Zone Code");

                entity.Property(e => e.DistZoneCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("DIST_ZONE_CD")
                    .HasComment("Zone Code");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DistCircleNavigation)
                    .WithMany(p => p.DistCdtabs)
                    .HasForeignKey(d => d.DistCircle)
                    .HasConstraintName("fk_DIST_CDTAB_KZN_CDTAB");
            });

            modelBuilder.Entity<TblEmprolehistDet>(entity =>
            {
                entity.HasKey(e => e.EmprolehistId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_emprolehist_det");

                entity.HasComment("Role History Details");

                entity.Property(e => e.EmprolehistId)
                    .HasColumnType("bigint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("emprolehist_id");

                entity.Property(e => e.EmpId)
                      .HasMaxLength(8)
                      .HasColumnName("emp_id");

                entity.Property(e => e.RoleId)
                    .HasColumnType("int")
                    .HasColumnName("role_id")
                    .HasComment("Role Id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<DscDetailstab>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("dsc_detailstab");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DscExpirtDate)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("DSC_EXPIRT_DATE");

                entity.Property(e => e.DscName)
                    .HasMaxLength(100)
                    .HasColumnName("DSC_NAME");

                entity.Property(e => e.DscPublicKey)
                    .HasMaxLength(300)
                    .HasColumnName("DSC_PUBLIC_KEY");

                entity.Property(e => e.DscSerno)
                    .HasMaxLength(50)
                    .HasColumnName("DSC_SERNO");

                entity.Property(e => e.DscrowId).HasColumnName("DSCROW_ID");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("EMP_ID");

                entity.Property(e => e.EmpPassword)
                    .HasMaxLength(250)
                    .HasColumnName("EMP_PASSWORD");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<EmpsessionTab>(entity =>
            {
                entity.HasKey(e => e.EmpsessionId)
                    .HasName("PRIMARY");

                entity.ToTable("empsession_tab");

                entity.HasComment("Employee Session Details");

                entity.Property(e => e.EmpsessionId)
                    .HasColumnType("tinyint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("EMPSESSION_ID");

                entity.Property(e => e.Accesstoken)
                    .HasMaxLength(2000)
                    .HasColumnName("ACCESSTOKEN");

                entity.Property(e => e.Accesstokenexpirydatetime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("ACCESSTOKENEXPIRYDATETIME");

                entity.Property(e => e.Accesstokenrevoke).HasColumnName("ACCESSTOKENREVOKE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("EMP_ID")
                    .HasComment("Employee ID");

                entity.Property(e => e.Ipadress)
                    .HasMaxLength(200)
                    .HasColumnName("IPADRESS")
                    .HasComment("To track device ip");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.LoginDateTime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("LOGIN_DATE_TIME")
                    .HasComment("Date and Time of Login");

                entity.Property(e => e.LogoutDateTime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("LOGOUT_DATE_TIME")
                    .HasComment("Date and Time of Logout");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Refreshtoken)
                    .HasMaxLength(2000)
                    .HasColumnName("REFRESHTOKEN");

                entity.Property(e => e.Refreshtokenexpirydatetime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("REFRESHTOKENEXPIRYDATETIME");

                entity.Property(e => e.Refreshtokenrevoke).HasColumnName("REFRESHTOKENREVOKE");

                entity.Property(e => e.SessionStatus)
                    .HasColumnName("SESSION_STATUS")
                    .HasComment("Status of Session");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<HobCdtab>(entity =>
            {
                entity.HasKey(e => e.HobCd)
                    .HasName("PRIMARY");

                entity.ToTable("hob_cdtab");

                entity.HasComment("Hobli Details");

                entity.HasIndex(e => e.TlqCd, "fk_HOB_CDTAB_TLQ_CDTAB");

                entity.Property(e => e.HobCd)
                    .HasColumnName("HOB_CD")
                    .HasComment("Hobli Code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.HobBhoomicode).HasColumnName("HOB_BHOOMICODE");

                entity.Property(e => e.HobLgdcode).HasColumnName("HOB_LGDCODE");

                entity.Property(e => e.HobNam)
                    .HasMaxLength(25)
                    .HasColumnName("HOB_NAM")
                    .HasComment("Hobli Name");

                entity.Property(e => e.HobNameKannada)
                    .HasMaxLength(50)
                    .HasColumnName("HOB_NAME_KANNADA")
                    .HasComment("Hobli Name in Kannada");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.TlqCd)
                    .HasColumnName("TLQ_CD")
                    .HasComment("Taluka Code Ref: tlq_cdtab (tlq_cd)");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.TlqCdNavigation)
                    .WithMany(p => p.HobCdtabs)
                    .HasForeignKey(d => d.TlqCd)
                    .HasConstraintName("fk_HOB_CDTAB_TLQ_CDTAB");
            });

            modelBuilder.Entity<IpCdtab>(entity =>
            {
                entity.HasKey(e => e.IpCd)
                    .HasName("PRIMARY");

                entity.ToTable("ip_cdtab");

                entity.HasComment("IP Address");

                entity.HasIndex(e => e.IpCd, "IP_CD_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.IpCd).HasColumnName("IP_CD");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(145)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Ip)
                    .HasMaxLength(100)
                    .HasColumnName("IP");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(145)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(45)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<KsfcuserDesignationDetail>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("ksfcuser_designation_details");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Employeeid)
                    .HasMaxLength(8)
                    .HasColumnName("EMPLOYEEID");

                entity.Property(e => e.InchargeDesigCode).HasColumnName("INCHARGE_DESIG_CODE");

                entity.Property(e => e.InchargeDesigDate)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("INCHARGE_DESIG_DATE");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PpDesigCode).HasColumnName("PP_DESIG_CODE");

                entity.Property(e => e.PpDesigDate)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("PP_DESIG_DATE");

                entity.Property(e => e.SubstantiveDesigCode).HasColumnName("SUBSTANTIVE_DESIG_CODE");

                entity.Property(e => e.SubstantiveDesigDate)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("SUBSTANTIVE_DESIG_DATE");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UserRowid).HasColumnName("USER_ROWID");
            });

            modelBuilder.Entity<KznCdtab>(entity =>
            {
                entity.HasKey(e => e.KznCd)
                    .HasName("PRIMARY");

                entity.ToTable("kzn_cdtab");

                entity.HasComment("Circle details");

                entity.Property(e => e.KznCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("KZN_CD")
                    .HasComment("Circle Code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.KznAdr1)
                    .HasMaxLength(30)
                    .HasColumnName("KZN_ADR1")
                    .HasComment("Address");

                entity.Property(e => e.KznAdr2)
                    .HasMaxLength(30)
                    .HasColumnName("KZN_ADR2")
                    .HasComment("Address");

                entity.Property(e => e.KznAdr3)
                    .HasMaxLength(30)
                    .HasColumnName("KZN_ADR3")
                    .HasComment("Address");

                entity.Property(e => e.KznFax)
                    .HasMaxLength(15)
                    .HasColumnName("KZN_FAX")
                    .HasComment("fax");

                entity.Property(e => e.KznFlag)
                    .HasColumnName("KZN_FLAG")
                    .HasComment("isActive/Inactive");

                entity.Property(e => e.KznNam)
                    .HasMaxLength(20)
                    .HasColumnName("KZN_NAM")
                    .HasComment("Circle Name");

                entity.Property(e => e.KznPin)
                    .HasColumnName("KZN_PIN")
                    .HasComment("Pincode");

                entity.Property(e => e.KznTel)
                    .HasColumnName("KZN_TEL")
                    .HasComment("Telephone");

                entity.Property(e => e.KznTlx)
                    .HasMaxLength(20)
                    .HasColumnName("KZN_TLX")
                    .HasComment("Telex");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<NuserhistoryTab>(entity =>
            {
                entity.HasKey(e => e.UserHistoryId)
                    .HasName("PRIMARY");

                entity.ToTable("nuserhistory_tab");

                entity.HasComment("New User History Details");

                entity.Property(e => e.UserHistoryId).HasColumnName("USER_HISTORY_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Process).HasMaxLength(45);

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UserRowId)
                    .HasColumnName("USER_ROW_ID")
                    .HasComment("User Row ID as in tbl_regduse");

                entity.Property(e => e.VerDate)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("VER_DATE")
                    .HasComment("Date of Verification");

                entity.Property(e => e.VerDetails)
                    .HasMaxLength(10)
                    .HasColumnName("VER_DETAILS")
                    .HasComment("Mobile No. / PAN No.");

                entity.Property(e => e.VerStatus)
                    .HasMaxLength(10)
                    .HasColumnName("VER_STATUS")
                    .IsFixedLength(true)
                    .HasComment("Verification Status (success / failed)");

                entity.Property(e => e.VerType)
                    .HasMaxLength(1)
                    .HasColumnName("VER_TYPE")
                    .IsFixedLength(true)
                    .HasComment("Verification  Type (Mobile (M) / Pan (P))");
            });

            modelBuilder.Entity<OffcCdtab>(entity =>
            {
                entity.HasKey(e => e.OffcCd)
                    .HasName("PRIMARY");

                entity.ToTable("offc_cdtab");

                entity.HasComment("Branch Office Details");

                entity.HasIndex(e => e.OffcDist, "fk_OFFC_DIST_DIST_CDTAB");

                entity.HasIndex(e => e.OffcZone, "fk_OFFC_DIST_KZN_CDTAB");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("OFFC_CD")
                    .HasComment("Branch Office Code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcAdr1)
                    .HasMaxLength(35)
                    .HasColumnName("OFFC_ADR1")
                    .HasComment("Office Address");

                entity.Property(e => e.OffcAdr2)
                    .HasMaxLength(35)
                    .HasColumnName("OFFC_ADR2")
                    .HasComment("Office Address");

                entity.Property(e => e.OffcAdr3)
                    .HasMaxLength(35)
                    .HasColumnName("OFFC_ADR3")
                    .HasComment("Office Address");

                entity.Property(e => e.OffcBmMailId)
                    .HasMaxLength(25)
                    .HasColumnName("OFFC_BM_MAIL_ID")
                    .HasComment("Branch Manager Mail ID");

                entity.Property(e => e.OffcBmcd)
                    .HasColumnName("OFFC_BMCD")
                    .HasComment("Branch Manager Code");

                entity.Property(e => e.OffcBsrCd)
                    .HasMaxLength(7)
                    .HasColumnName("OFFC_BSR_CD")
                    .HasComment("Bank Branch Code");

                entity.Property(e => e.OffcDist)
                    .HasColumnType("tinyint")
                    .HasColumnName("OFFC_DIST")
                    .HasComment("District Code Ref: dist_cdtab (dist_cd)");

                entity.Property(e => e.OffcFax)
                    .HasColumnName("OFFC_FAX")
                    .HasComment("Fax");

                entity.Property(e => e.OffcIfsCd)
                    .HasMaxLength(11)
                    .HasColumnName("OFFC_IFS_CD")
                    .HasComment("Branch Bank IFSC");

                entity.Property(e => e.OffcInopbnkacNo)
                    .HasMaxLength(13)
                    .HasColumnName("OFFC_INOPBNKAC_NO")
                    .HasComment("Branch Bank Account No.");

                entity.Property(e => e.OffcMailId)
                    .HasMaxLength(15)
                    .HasColumnName("OFFC_MAIL_ID")
                    .HasComment("Branch Mail ID (used for Demand Notice)");

                entity.Property(e => e.OffcNam)
                    .HasMaxLength(30)
                    .HasColumnName("OFFC_NAM")
                    .HasComment("Name of Branch Office");

                entity.Property(e => e.OffcNamKannada)
                    .HasMaxLength(100)
                    .HasColumnName("OFFC_NAM_KANNADA");

                entity.Property(e => e.OffcPin)
                    .HasColumnName("OFFC_PIN")
                    .HasComment("Pincode");

                entity.Property(e => e.OffcStNo)
                    .HasMaxLength(15)
                    .HasColumnName("OFFC_ST_NO")
                    .HasComment("Branch Service Tax no.");

                entity.Property(e => e.OffcStdCd)
                    .HasMaxLength(5)
                    .HasColumnName("OFFC_STD_CD")
                    .HasComment("Branch STD Code");

                entity.Property(e => e.OffcTaxNo)
                    .HasMaxLength(10)
                    .HasColumnName("OFFC_TAX_NO")
                    .HasComment("Branch TAN");

                entity.Property(e => e.OffcTel1)
                    .HasColumnName("OFFC_TEL1")
                    .HasComment("Telephone");

                entity.Property(e => e.OffcTel2)
                    .HasColumnName("OFFC_TEL2")
                    .HasComment("Telephone");

                entity.Property(e => e.OffcTel3)
                    .HasColumnName("OFFC_TEL3")
                    .HasComment("Telephone");

                entity.Property(e => e.OffcTlx2)
                    .HasColumnName("OFFC_TLX2")
                    .HasComment("Telex");

                entity.Property(e => e.OffcZone)
                    .HasColumnType("tinyint")
                    .HasColumnName("OFFC_ZONE")
                    .HasComment("Circle Code Ref: kzn_cdtab (kzn_cd)");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.OffcDistNavigation)
                    .WithMany(p => p.OffcCdtabs)
                    .HasForeignKey(d => d.OffcDist)
                    .HasConstraintName("fk_OFFC_DIST_DIST_CDTAB");

                entity.HasOne(d => d.OffcZoneNavigation)
                    .WithMany(p => p.OffcCdtabs)
                    .HasForeignKey(d => d.OffcZone)
                    .HasConstraintName("fk_OFFC_DIST_KZN_CDTAB");

                entity.HasOne(e => e.TblAppLoanMast)
                       .WithOne(b => b.OffcCdtab)
                       .HasForeignKey<TblAppLoanMast>(b => b.InOffc);
            });

            modelBuilder.Entity<OtpTab>(entity =>
            {
                entity.HasKey(e => e.OtpId)
                    .HasName("PRIMARY");

                entity.ToTable("otp_tab");

                entity.HasComment("OTP Details");

                entity.Property(e => e.OtpId).HasColumnName("OTP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.MobileNo)
                    .HasMaxLength(10)
                    .HasColumnName("MOBILE_NO")
                    .HasComment("Mobile No.");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Otp)
                    .HasMaxLength(6)
                    .HasColumnName("OTP")
                    .HasComment("One Time Pin");

                entity.Property(e => e.Otpexpirationdatetime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("OTPEXPIRATIONDATETIME");

                entity.Property(e => e.Process)
                    .HasMaxLength(10)
                    .HasColumnName("PROCESS")
                    .IsFixedLength(true)
                    .HasComment("which process otp is generated");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UserId)
                    .HasMaxLength(10)
                    .HasColumnName("USER_ID")
                    .HasComment("Employee ID / PAN");

                entity.Property(e => e.VerStatus)
                    .HasColumnName("VER_STATUS")
                    .HasComment("Status of Verification");
            });

            modelBuilder.Entity<PromUnitMaptab>(entity =>
            {
                entity.HasKey(e => e.PromUnitId)
                    .HasName("PRIMARY");

                entity.ToTable("prom_unit_maptab");

                entity.HasComment("Promoter - Unit Mapping Table");

                entity.Property(e => e.PromUnitId)
                    .HasColumnType("tinyint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PROM_UNIT_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromCode)
                    .HasColumnName("PROM_CODE")
                    .HasComment("New Promoter Code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UtCd)
                    .HasColumnName("UT_CD")
                    .HasComment("New Unit ID");
            });

            modelBuilder.Entity<Promoter>(entity =>
            {
                entity.HasNoKey();

                entity.ToTable("promoter");

                entity.HasComment("Promoter Details");

                entity.HasIndex(e => e.PromOffc, "fk_PROMOTER_OFFC_CDTAB");

                entity.HasIndex(e => e.PromUnit, "fk_PROMOTER_UNIT_INFO1");

                entity.HasIndex(e => e.PromoterCode, "fk_PROMOTER_tbl_prom_cdtab");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PanNo)
                    .HasMaxLength(15)
                    .HasColumnName("PAN_NO")
                    .HasComment("PAN No");

                entity.Property(e => e.PassNo)
                    .HasMaxLength(15)
                    .HasColumnName("PASS_NO")
                    .HasComment("Passport No.");

                entity.Property(e => e.PromAadhaar)
                    .HasMaxLength(300)
                    .HasColumnName("PROM_AADHAAR")
                    .HasComment("Promoter Aadhaar (Secured Folder Path)");

                entity.Property(e => e.PromAge)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_AGE")
                    .HasComment("Promoter Age");

                entity.Property(e => e.PromClas1)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_CLAS1")
                    .HasComment("Class Code Ref: xxxxxxx");

                entity.Property(e => e.PromClas2)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_CLAS2")
                    .HasComment("Class Code Ref: xxxxxxx");

                entity.Property(e => e.PromCode)
                    .HasColumnName("PROM_CODE")
                    .HasComment("Promoter Code");

                entity.Property(e => e.PromDesg)
                    .HasMaxLength(25)
                    .HasColumnName("PROM_DESG")
                    .HasComment("Promoter Designation");

                entity.Property(e => e.PromDob)
                    .HasColumnName("PROM_DOB")
                    .HasComment("Promoter Date of Birth");

                entity.Property(e => e.PromDom1)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_DOM1")
                    .HasComment("Domicile Code Ref: xxxxxxx");

                entity.Property(e => e.PromEmail)
                    .HasMaxLength(40)
                    .HasColumnName("PROM_EMAIL")
                    .HasComment("Promoter E-Mail");

                entity.Property(e => e.PromExAppBy)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_EX_APP_BY")
                    .HasComment("Approved by - Designation");

                entity.Property(e => e.PromExApprEmp)
                    .HasColumnName("PROM_EX_APPR_EMP")
                    .HasComment("Approved by Employee code");

                entity.Property(e => e.PromExDt)
                    .HasColumnName("PROM_EX_DT")
                    .HasComment("Date of Exit");

                entity.Property(e => e.PromExpDet)
                    .HasMaxLength(250)
                    .HasColumnName("PROM_EXP_DET")
                    .HasComment("Experience Details Date of Joining");

                entity.Property(e => e.PromExpYrs)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_EXP_YRS")
                    .HasComment("Years of Experience");

                entity.Property(e => e.PromGuardian)
                    .HasMaxLength(35)
                    .HasColumnName("PROM_GUARDIAN")
                    .HasComment("Name of Guardian");

                entity.Property(e => e.PromJnDt).HasColumnName("PROM_JN_DT");

                entity.Property(e => e.PromMajor)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_MAJOR")
                    .HasComment("Whether Major Promoter");

                entity.Property(e => e.PromMobile)
                    .HasMaxLength(15)
                    .HasColumnName("PROM_MOBILE")
                    .HasComment("Promoter Mobile No.");

                entity.Property(e => e.PromName)
                    .HasMaxLength(35)
                    .HasColumnName("PROM_NAME")
                    .HasComment("Name of Promoter");

                entity.Property(e => e.PromNriCountry)
                    .HasMaxLength(100)
                    .HasColumnName("PROM_NRI_COUNTRY")
                    .HasComment("Name of Country");

                entity.Property(e => e.PromNwDets)
                    .HasMaxLength(800)
                    .HasColumnName("PROM_NW_DETS")
                    .HasComment("Net Worth Details");

                entity.Property(e => e.PromOffc)
                    .HasColumnType("tinyint")
                    .HasColumnName("PROM_OFFC")
                    .HasComment("Branch Code Ref: offc_cdtab (offc_cd)");

                entity.Property(e => e.PromPadr1)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_PADR1")
                    .HasComment("Permanent Address 1");

                entity.Property(e => e.PromPadr2)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_PADR2")
                    .HasComment("Permanent Address 2");

                entity.Property(e => e.PromPadr3)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_PADR3")
                    .HasComment("Permanent Address 3");

                entity.Property(e => e.PromPadr4)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_PADR4")
                    .HasComment("Permanent Address 4");

                entity.Property(e => e.PromPhyHandicap)
                    .HasMaxLength(1)
                    .HasColumnName("PROM_PHY_HANDICAP")
                    .HasComment("Whether Physically Handicap");

                entity.Property(e => e.PromQual1)
                    .HasMaxLength(10)
                    .HasColumnName("PROM_QUAL1")
                    .HasComment("Qualification 1");

                entity.Property(e => e.PromQual2)
                    .HasMaxLength(10)
                    .HasColumnName("PROM_QUAL2")
                    .HasComment("Qualification 2");

                entity.Property(e => e.PromResTel)
                    .HasColumnName("PROM_RES_TEL")
                    .HasComment("Residence Telephone No");

                entity.Property(e => e.PromSex)
                    .HasMaxLength(1)
                    .HasColumnName("PROM_SEX")
                    .HasComment("Gender of Promoter");

                entity.Property(e => e.PromShare)
                    .HasColumnType("decimal(5,2)")
                    .HasColumnName("PROM_SHARE")
                    .HasComment("Share of Promoter");

                entity.Property(e => e.PromTadr1)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_TADR1")
                    .HasComment("Temporary Address 1");

                entity.Property(e => e.PromTadr2)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_TADR2")
                    .HasComment("Temporary Address 2");

                entity.Property(e => e.PromTadr3)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_TADR3")
                    .HasComment("Temporary Address 3");

                entity.Property(e => e.PromTadr4)
                    .HasMaxLength(30)
                    .HasColumnName("PROM_TADR4")
                    .HasComment("Temporary Address 4");

                entity.Property(e => e.PromUnit)
                    .HasColumnName("PROM_UNIT")
                    .HasComment("Unit Code Ref: unit_info1 (ut_cd)");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.PromOffcNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.PromOffc)
                    .HasConstraintName("fk_PROMOTER_OFFC_CDTAB");

                entity.HasOne(d => d.PromUnitNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.PromUnit)
                    .HasConstraintName("fk_PROMOTER_UNIT_INFO1");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany()
                    .HasForeignKey(d => d.PromoterCode)
                    .HasConstraintName("fk_PROMOTER_tbl_prom_cdtab");
            });

            modelBuilder.Entity<PromoterMaptab>(entity =>
            {
                entity.HasKey(e => e.PromoterMapId)
                    .HasName("PRIMARY");

                entity.ToTable("promoter_maptab");

                entity.HasComment("Promoter Mapping Table");

                entity.Property(e => e.PromoterMapId)
                    .HasColumnType("tinyint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("PROMOTER_MAP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("OFFC_CD")
                    .HasComment("Branch Office Code");

                entity.Property(e => e.PromCode)
                    .HasColumnName("PROM_CODE")
                    .HasComment("New Promoter Code");

                entity.Property(e => e.PromCodeOld)
                    .HasColumnName("PROM_CODE_OLD")
                    .HasComment("Old Promoter Code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<PromsessionTab>(entity =>
            {
                entity.HasKey(e => e.PromsessionId)
                    .HasName("PRIMARY");

                entity.ToTable("promsession_tab");

                entity.HasComment("Promoter Session Details");

                entity.Property(e => e.PromsessionId).HasColumnName("PROMSESSION_ID");

                entity.Property(e => e.Accesstoken)
                    .HasMaxLength(2000)
                    .HasColumnName("ACCESSTOKEN");

                entity.Property(e => e.Accesstokenexpirydatetime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("ACCESSTOKENEXPIRYDATETIME");

                entity.Property(e => e.Accesstokenrevoke).HasColumnName("ACCESSTOKENREVOKE");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Ipadress)
                    .HasMaxLength(200)
                    .HasColumnName("IPADRESS")
                    .HasComment("To track device ip	");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.LoginDateTime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("LOGIN_DATE_TIME")
                    .HasComment("Date and Time of Login");

                entity.Property(e => e.LogoutDateTime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("LOGOUT_DATE_TIME")
                    .HasComment("Date and Time of Logout");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromPan)
                    .HasMaxLength(10)
                    .HasColumnName("PROM_PAN")
                    .HasComment("Employee ID");

                entity.Property(e => e.Refreshtoken)
                    .HasMaxLength(2000)
                    .HasColumnName("REFRESHTOKEN");

                entity.Property(e => e.Refreshtokenexpirydatetime)
                    .HasColumnType("datetime(6)")
                    .HasColumnName("REFRESHTOKENEXPIRYDATETIME");

                entity.Property(e => e.Refreshtokenrevoke).HasColumnName("REFRESHTOKENREVOKE");

                entity.Property(e => e.SessionStatus)
                    .HasColumnName("SESSION_STATUS")
                    .HasComment("Status of Session");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<RegduserTab>(entity =>
            {
                entity.HasKey(e => e.UserRowId)
                    .HasName("PRIMARY");

                entity.ToTable("regduser_tab");

                entity.HasComment("Registered User Details");

                entity.Property(e => e.UserRowId).HasColumnName("USER_ROW_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UserMobileno)
                    .HasMaxLength(10)
                    .HasColumnName("USER_MOBILENO")
                    .HasComment("User Mobile No.");

                entity.Property(e => e.UserPan)
                    .HasMaxLength(10)
                    .HasColumnName("USER_PAN")
                    .HasComment("User PAN");

                entity.Property(e => e.UserRegnDate)
                    .HasColumnName("USER_REGN_DATE")
                    .HasComment("User Regisered Date");
            });

            modelBuilder.Entity<TblAddressCdtab>(entity =>
            {
                entity.HasKey(e => e.AddtypeCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_address_cdtab");

                entity.Property(e => e.AddtypeCd).HasColumnName("addtype_cd");

                entity.Property(e => e.AddtypeDets)
                    .HasMaxLength(30)
                    .HasColumnName("addtype_dets");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblAssetcatCdtab>(entity =>
            {
                entity.HasKey(e => e.AssetcatCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_assetcat_cdtab");

                entity.Property(e => e.AssetcatCd).HasColumnName("assetcat_cd");

                entity.Property(e => e.AssetcatDets)
                    .HasMaxLength(100)
                    .HasColumnName("assetcat_dets");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblAssettypeCdtab>(entity =>
            {
                entity.HasKey(e => e.AssettypeCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_assettype_cdtab");

                entity.HasIndex(e => e.AssetcatCd, "fk_tbl_assettype_cdtab_tbl_assetcat_cdtab");

                entity.Property(e => e.AssettypeCd).HasColumnName("assettype_cd");

                entity.Property(e => e.AssetcatCd).HasColumnName("assetcat_cd");

                entity.Property(e => e.AssettypeDets)
                    .HasMaxLength(100)
                    .HasColumnName("assettype_dets");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SeqNo)
                    .HasColumnType("decimal(3,2)")
                    .HasColumnName("seq_no");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.AssetcatCdNavigation)
                    .WithMany(p => p.TblAssettypeCdtabs)
                    .HasForeignKey(d => d.AssetcatCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_assettype_cdtab_tbl_assetcat_cdtab");
            });

            modelBuilder.Entity<TblAttrCdtab>(entity =>
            {
                entity.HasKey(e => e.AttrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_attr_cdtab");

                entity.HasIndex(e => e.RoleId, "fk_tbl_attr_cdtab_tbl_role_cdtab");

                entity.Property(e => e.AttrId).HasColumnName("attr_id");

                entity.Property(e => e.AttrDesc)
                    .HasMaxLength(100)
                    .HasColumnName("attr_desc");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblAttrCdtabs)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_attr_cdtab_tbl_role_cdtab");
            });

            modelBuilder.Entity<TblAttrunitCdtab>(entity =>
            {
                entity.HasKey(e => e.AttrunitId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_attrunit_cdtab");

                entity.Property(e => e.AttrunitId).HasColumnName("attrunit_id");

                entity.Property(e => e.AttrunitDesc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("attrunit_desc");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblBankfacilityCdtab>(entity =>
            {
                entity.HasKey(e => e.BfacilityCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_bankfacility_cdtab");

                entity.Property(e => e.BfacilityCode).HasColumnName("bfacility_code");

                entity.Property(e => e.BfacilityDesc)
                    .HasMaxLength(50)
                    .HasColumnName("bfacility_desc");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(2)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("bit(1)")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(2)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(2)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblBuildingCdtab>(entity =>
            {
                entity.HasKey(e => e.BldCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_building_cdtab");

                entity.Property(e => e.BldCd).HasColumnName("bld_cd");

                entity.Property(e => e.BldDesc)
                    .HasMaxLength(30)
                    .HasColumnName("bld_desc");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblChairCdtab>(entity =>
            {
                entity.HasKey(e => e.ChairCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_chair_cdtab");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_chair_cdtab_OFFC_CDTAB");

                entity.Property(e => e.ChairCode).HasColumnName("chair_code");

                entity.Property(e => e.ChairDesc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("chair_desc");

                entity.Property(e => e.ChairId).HasColumnName("chair_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.OffcCdNavigation)
                    .WithMany(p => p.TblChairCdtabs)
                    .HasForeignKey(d => d.OffcCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_chair_cdtab_OFFC_CDTAB");
            });

            modelBuilder.Entity<TblChairmapCdtab>(entity =>
            {
                entity.HasKey(e => e.ChairmapId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_chairmap_cdtab");

                entity.HasIndex(e => e.FromChaircode, "fk_tbl_chairmap_cdtab_tbl_chair_cdtab_1");

                entity.HasIndex(e => e.ToChaircode, "fk_tbl_chairmap_cdtab_tbl_chair_cdtab_2");

                entity.Property(e => e.ChairmapId).HasColumnName("chairmap_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.FromChaircode).HasColumnName("from_chaircode");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ToChaircode).HasColumnName("to_chaircode");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.FromChaircodeNavigation)
                    .WithMany(p => p.TblChairmapCdtabFromChaircodeNavigations)
                    .HasForeignKey(d => d.FromChaircode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_chairmap_cdtab_tbl_chair_cdtab_1");

                entity.HasOne(d => d.ToChaircodeNavigation)
                    .WithMany(p => p.TblChairmapCdtabToChaircodeNavigations)
                    .HasForeignKey(d => d.ToChaircode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_chairmap_cdtab_tbl_chair_cdtab_2");
            });

            modelBuilder.Entity<TblCnstCdtab>(entity =>
            {
                entity.HasKey(e => e.CnstCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cnst_cdtab");

                entity.Property(e => e.CnstCd).HasColumnName("cnst_cd");

                entity.Property(e => e.CnstDets)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("cnst_dets");

                entity.Property(e => e.CnstPanchar)
                    .IsRequired()
                    .HasColumnName("cnst_panchar");

                entity.Property(e => e.CnstTaxr)
                    .HasColumnType("decimal(5,2)")
                    .HasColumnName("cnst_taxr");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblCondCdtab>(entity =>
            {
                entity.HasKey(e => e.CondCd)
                    .HasName("PRIMARY");


                entity.ToTable("tbl_cond_cdtab");

                entity.Property(e => e.CondCd).HasColumnName("cond_cd");

                entity.Property(e => e.CondDets)
                    .HasMaxLength(675)
                    .HasColumnName("cond_dets");

                entity.Property(e => e.CondFlg).HasColumnName("cond_flg");

                entity.Property(e => e.CondStatusFlag).HasColumnName("cond_status_flag");

                entity.Property(e => e.CondStg)
                    .HasColumnType("tinyint")
                    .HasColumnName("cond_stg");

                entity.Property(e => e.CondSub).HasColumnName("cond_sub");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

            });

            modelBuilder.Entity<TblDoccatCdtab>(entity =>
            {
                entity.HasKey(e => e.DoccatCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_doccat_cdtab");

                entity.Property(e => e.DoccatCd).HasColumnName("doccat_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DoccatDets)
                    .HasMaxLength(100)
                    .HasColumnName("doccat_dets");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblDocdetailsCdtab>(entity =>
            {
                entity.HasKey(e => e.DocdetCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_docdetails_cdtab");

                entity.HasIndex(e => e.DoccatCd, "fk_tbl_docdetails_cdtab_tbl_doccat_cdtab");

                entity.Property(e => e.DocdetCd).HasColumnName("docdet_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DoccatCd).HasColumnName("doccat_cd");

                entity.Property(e => e.DocdetDets)
                    .HasMaxLength(500)
                    .HasColumnName("docdet_dets");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SeqNo)
                    .HasColumnType("decimal(3,2)")
                    .HasColumnName("seq_no");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DoccatCdNavigation)
                    .WithMany(p => p.TblDocdetailsCdtabs)
                    .HasForeignKey(d => d.DoccatCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_docdetails_cdtab_tbl_doccat_cdtab");
            });

            modelBuilder.Entity<TblDomiCdtab>(entity =>
            {
                entity.HasKey(e => e.DomCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_domi_cdtab");

                entity.Property(e => e.DomCd).HasColumnName("dom_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DomDets)
                    .HasMaxLength(30)
                    .HasColumnName("dom_dets");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblDopCdtab>(entity =>
            {
                entity.HasKey(e => e.DopId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dop_cdtab");

                entity.HasIndex(e => e.AttrId, "fk_tbl_dop_cdtab_tbl_attr_cdtab");

                entity.HasIndex(e => e.RoleId, "fk_tbl_dop_cdtab_tbl_role_cdtab");

                entity.HasIndex(e => e.SubattrId, "fk_tbl_dop_cdtab_tbl_subattr_cdtab");

                entity.HasIndex(e => e.TgesCode, "fk_tbl_dop_cdtab_tbl_trg_emp_grade");

                entity.Property(e => e.DopId).HasColumnName("dop_id");

                entity.Property(e => e.AttrId).HasColumnName("attr_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DopValue).HasColumnName("dop_value");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SubattrId).HasColumnName("subattr_id");

                entity.Property(e => e.TgesCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("tges_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Attr)
                    .WithMany(p => p.TblDopCdtabs)
                    .HasForeignKey(d => d.AttrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dop_cdtab_tbl_attr_cdtab");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblDopCdtabs)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dop_cdtab_tbl_role_cdtab");

                entity.HasOne(d => d.Subattr)
                    .WithMany(p => p.TblDopCdtabs)
                    .HasForeignKey(d => d.SubattrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dop_cdtab_tbl_subattr_cdtab");

                entity.HasOne(d => d.TgesCodeNavigation)
                    .WithMany(p => p.TblDopCdtabs)
                    .HasForeignKey(d => d.TgesCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dop_cdtab_tbl_trg_emp_grade");
            });

            modelBuilder.Entity<TblDophistDet>(entity =>
            {
                entity.HasKey(e => e.DophistId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_dophist_det");

                entity.HasIndex(e => e.AttrId, "fk_tbl_dophist_det_tbl_attr_cdtab");

                entity.HasIndex(e => e.RoleId, "fk_tbl_dophist_det_tbl_role_cdtab");

                entity.HasIndex(e => e.SubattrId, "fk_tbl_dophist_det_tbl_subattr_cdtab");

                entity.HasIndex(e => e.TgesCode, "fk_tbl_dophist_det_tbl_trg_emp_grade");

                entity.Property(e => e.DophistId).HasColumnName("dophist_id");

                entity.Property(e => e.AttrId).HasColumnName("attr_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DopValue).HasColumnName("dop_value");

                entity.Property(e => e.FromDate)
                    .HasColumnType("date")
                    .HasColumnName("from_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.SubattrId).HasColumnName("subattr_id");

                entity.Property(e => e.TgesCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("tges_code");

                entity.Property(e => e.ToDate)
                    .HasColumnType("date")
                    .HasColumnName("to_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Attr)
                    .WithMany(p => p.TblDophistDets)
                    .HasForeignKey(d => d.AttrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dophist_det_tbl_attr_cdtab");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.TblDophistDets)
                    .HasForeignKey(d => d.RoleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dophist_det_tbl_role_cdtab");

                entity.HasOne(d => d.Subattr)
                    .WithMany(p => p.TblDophistDets)
                    .HasForeignKey(d => d.SubattrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dophist_det_tbl_subattr_cdtab");

                entity.HasOne(d => d.TgesCodeNavigation)
                    .WithMany(p => p.TblDophistDets)
                    .HasForeignKey(d => d.TgesCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_dophist_det_tbl_trg_emp_grade");
            });

            modelBuilder.Entity<TblEgStatusMast>(entity =>
            {
                entity.HasKey(e => e.EsmCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_eg_status_mast");

                entity.Property(e => e.EsmCode).HasColumnName("esm_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EsmDescription)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("esm_description");

                entity.Property(e => e.EsmFlag).HasColumnName("esm_flag");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblEmpchairDet>(entity =>
            {
                entity.HasKey(e => e.EmpchairId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_empchair_det");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_empchair_det_OFFC_CDTAB");

                entity.HasIndex(e => e.ChairCode, "fk_tbl_empchair_det_tbl_chair_cdtab");

                entity.HasIndex(e => e.TgesCode, "fk_tbl_empchair_det_tbl_trg_emp_grade");

                entity.HasIndex(e => e.EmpId, "fk_tbl_empchair_det_tbl_trg_employee");

                entity.Property(e => e.EmpchairId).HasColumnName("empchair_id");

                entity.Property(e => e.ChairCode).HasColumnName("chair_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("offc_cd");

                entity.Property(e => e.TgesCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("tges_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.FromDate)
                   .HasColumnName("from_date");

                entity.HasOne(d => d.ChairCodeNavigation)
                    .WithMany(p => p.TblEmpchairDets)
                    .HasForeignKey(d => d.ChairCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empchair_det_tbl_chair_cdtab");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmpchairDets)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("fk_tbl_empchair_det_tbl_trg_employee");

                entity.HasOne(d => d.OffcCdNavigation)
                    .WithMany(p => p.TblEmpchairDets)
                    .HasForeignKey(d => d.OffcCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empchair_det_OFFC_CDTAB");

                entity.HasOne(d => d.TgesCodeNavigation)
                    .WithMany(p => p.TblEmpchairDets)
                    .HasForeignKey(d => d.TgesCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empchair_det_tbl_trg_emp_grade");

                /// <summary>
                ///  Author: Gowtham S; Date:2/08/2022
                /// </summary>

                // entity.HasOne(e => e.TblIdmLegalWorkflow)
                //   .WithOne(b => b.TblEmpchairDets)
                //   .HasForeignKey<TblEmpchairDet>(b => b.OffcCd);
            });

            modelBuilder.Entity<TblEmpchairhistDet>(entity =>
            {
                entity.HasKey(e => e.EmpchairhistId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_empchairhist_det");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_empchairhist_det_OFFC_CDTAB");

                entity.HasIndex(e => e.ChairCode, "fk_tbl_empchairhist_det_tbl_chair_cdtab");

                entity.HasIndex(e => e.TgesCode, "fk_tbl_empchairhist_det_tbl_trg_emp_grade");

                entity.HasIndex(e => e.EmpId, "fk_tbl_empchairhist_det_tbl_trg_employee");

                entity.Property(e => e.EmpchairhistId).HasColumnName("empchairhist_id");

                entity.Property(e => e.ChairCode).HasColumnName("chair_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.FromDate)
                    .HasColumnType("date")
                    .HasColumnName("from_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("offc_cd");

                entity.Property(e => e.TgesCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("tges_code");

                entity.Property(e => e.ToDate)
                    .HasColumnType("date")
                    .HasColumnName("to_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.ChairCodeNavigation)
                    .WithMany(p => p.TblEmpchairhistDets)
                    .HasForeignKey(d => d.ChairCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empchairhist_det_tbl_chair_cdtab");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmpchairhistDets)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("fk_tbl_empchairhist_det_tbl_trg_employee");

                entity.HasOne(d => d.OffcCdNavigation)
                    .WithMany(p => p.TblEmpchairhistDets)
                    .HasForeignKey(d => d.OffcCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empchairhist_det_OFFC_CDTAB");

                entity.HasOne(d => d.TgesCodeNavigation)
                    .WithMany(p => p.TblEmpchairhistDets)
                    .HasForeignKey(d => d.TgesCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empchairhist_det_tbl_trg_emp_grade");
            });

            modelBuilder.Entity<TblEmpdesigTab>(entity =>
            {
                entity.HasKey(e => e.EmpdesigId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_empdesig_tab");

                entity.HasIndex(e => e.SubstDesigCode, "fk_tbl_empdesig_tab_tbl_trg_emp_grade_1");

                entity.HasIndex(e => e.PpDesignCode, "fk_tbl_empdesig_tab_tbl_trg_emp_grade_2");

                entity.HasIndex(e => e.IcDesigCode, "fk_tbl_empdesig_tab_tbl_trg_emp_grade_3");

                entity.HasIndex(e => e.EmpId, "fk_tbl_empdesig_tab_tbl_trg_employee");

                entity.Property(e => e.EmpdesigId).HasColumnName("empdesig_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpId)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.IcDate)
                    .HasColumnType("date")
                    .HasColumnName("ic_date");

                entity.Property(e => e.IcDesigCode)
                    .HasMaxLength(5)
                    .HasColumnName("ic_desig_code");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PpDate)
                    .HasColumnType("date")
                    .HasColumnName("pp_date");

                entity.Property(e => e.PpDesignCode)
                    .HasMaxLength(5)
                    .HasColumnName("pp_design_code");

                entity.Property(e => e.SubstDate)
                    .HasColumnType("date")
                    .HasColumnName("subst_date");

                entity.Property(e => e.SubstDesigCode)
                    .HasMaxLength(5)
                    .HasColumnName("subst_desig_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmpdesigTabs)
                    .HasForeignKey(d => d.EmpId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_empdesig_tab_tbl_trg_employee");

                entity.HasOne(d => d.IcDesigCodeNavigation)
                    .WithMany(p => p.TblEmpdesigTabIcDesigCodeNavigations)
                    .HasForeignKey(d => d.IcDesigCode)
                    .HasConstraintName("fk_tbl_empdesig_tab_tbl_trg_emp_grade_3");

                entity.HasOne(d => d.PpDesignCodeNavigation)
                    .WithMany(p => p.TblEmpdesigTabPpDesignCodeNavigations)
                    .HasForeignKey(d => d.PpDesignCode)
                    .HasConstraintName("fk_tbl_empdesig_tab_tbl_trg_emp_grade_2");

                entity.HasOne(d => d.SubstDesigCodeNavigation)
                    .WithMany(p => p.TblEmpdesigTabSubstDesigCodeNavigations)
                    .HasForeignKey(d => d.SubstDesigCode)
                    .HasConstraintName("fk_tbl_empdesig_tab_tbl_trg_emp_grade_1");
            });

            modelBuilder.Entity<TblEmpdesighistTab>(entity =>
            {
                entity.HasKey(e => e.EmpdesighistId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_empdesighist_tab");

                entity.HasIndex(e => e.DesigCode, "fk_tbl_empdesighist_tab_tbl_trg_emp_grade");

                entity.HasIndex(e => e.EmpId, "fk_tbl_empdesighist_tab_tbl_trg_employee");

                entity.Property(e => e.EmpdesighistId).HasColumnName("empdesighist_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DesigCode)
                    .HasMaxLength(5)
                    .HasColumnName("desig_code");

                entity.Property(e => e.DesigTypeCode)
                    .HasMaxLength(10)
                    .HasColumnName("desig_type_code");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.FromDate)
                    .HasColumnType("date")
                    .HasColumnName("from_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ToDate)
                    .HasColumnType("date")
                    .HasColumnName("to_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DesigCodeNavigation)
                    .WithMany(p => p.TblEmpdesighistTabs)
                    .HasForeignKey(d => d.DesigCode)
                    .HasConstraintName("fk_tbl_empdesighist_tab_tbl_trg_emp_grade");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmpdesighistTabs)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("fk_tbl_empdesighist_tab_tbl_trg_employee");
            });

            modelBuilder.Entity<TblEmpdscTab>(entity =>
            {
                entity.HasKey(e => e.EmpuserId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_empdsc_tab");

                entity.HasIndex(e => e.EmpId, "fk_tbl_empdsc_tab_tbl_trg_employee");

                entity.Property(e => e.EmpuserId).HasColumnName("empuser_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DscExpdate)
                    .HasColumnType("date")
                    .HasColumnName("dsc_expdate");

                entity.Property(e => e.DscName)
                    .HasMaxLength(100)
                    .HasColumnName("dsc_name");

                entity.Property(e => e.DscPubkey)
                    .HasMaxLength(300)
                    .HasColumnName("dsc_pubkey");

                entity.Property(e => e.DscSlno).HasColumnName("dsc_slno");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.EmpPswd)
                    .HasMaxLength(500)
                    .HasColumnName("emp_pswd");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.IsPswdChng).HasColumnName("is_pswd_chng");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmpdscTabs)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("fk_tbl_empdsc_tab_tbl_trg_employee");
            });

            modelBuilder.Entity<TblEmploginTab>(entity =>
            {
                entity.HasKey(e => e.EmploginId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_emplogin_tab");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_emplogin_tab_OFFC_CDTAB");

                entity.HasIndex(e => e.EmpId, "fk_tbl_emplogin_tab_tbl_trg_employee");

                entity.Property(e => e.EmploginId).HasColumnName("emplogin_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.LoginDatetime)
                    .HasColumnType("date")
                    .HasColumnName("login_datetime");

                entity.Property(e => e.LogoutDatetime)
                    .HasColumnType("date")
                    .HasColumnName("Logout_datetime");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Emp)
                    .WithMany(p => p.TblEmploginTabs)
                    .HasForeignKey(d => d.EmpId)
                    .HasConstraintName("fk_tbl_emplogin_tab_tbl_trg_employee");

                entity.HasOne(d => d.OffcCdNavigation)
                    .WithMany(p => p.TblEmploginTabs)
                    .HasForeignKey(d => d.OffcCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_emplogin_tab_OFFC_CDTAB");
            });

            modelBuilder.Entity<TblEnqAddressDet>(entity =>
            {
                entity.HasKey(e => e.EnqAddresssId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_address_det");

                entity.HasIndex(e => e.AddtypeCd, "fk_bl_enq_address_det_tbl_address_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_address_det_tbl_enq_temptab");

                entity.Property(e => e.EnqAddresssId).HasColumnName("enq_addresss_id");

                entity.Property(e => e.AddtypeCd).HasColumnName("addtype_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniitAddress)
                    .HasMaxLength(200)
                    .HasColumnName("uniit_address");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UnitEmail)
                    .HasMaxLength(50)
                    .HasColumnName("unit_email");

                entity.Property(e => e.UnitFax).HasColumnName("unit_fax");

                entity.Property(e => e.UnitMobileNo).HasColumnName("unit_mobile_no");

                entity.Property(e => e.UnitPincode).HasColumnName("unit_pincode");

                entity.Property(e => e.UnitTelNo).HasColumnName("unit_tel_no");

                entity.HasOne(d => d.AddtypeCdNavigation)
                    .WithMany(p => p.TblEnqAddressDets)
                    .HasForeignKey(d => d.AddtypeCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_bl_enq_address_det_tbl_address_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqAddressDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_address_det_tbl_enq_temptab");
            });

            modelBuilder.Entity<TblEnqBankDet>(entity =>
            {
                entity.HasKey(e => e.EnqBankId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_bank_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_bank_det_tbl_enq_temptab");

                entity.Property(e => e.EnqBankId).HasColumnName("enq_bank_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqAccName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("enq_acc_name");

                entity.Property(e => e.EnqAcctype)
                    .HasMaxLength(20)
                    .HasColumnName("enq_acctype");

                entity.Property(e => e.EnqBankaccno)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("enq_bankaccno");

                entity.Property(e => e.EnqBankbr)
                    .HasMaxLength(100)
                    .HasColumnName("enq_bankbr");

                entity.Property(e => e.EnqBankname)
                    .HasMaxLength(100)
                    .HasColumnName("enq_bankname");

                entity.Property(e => e.EnqIfsc)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("enq_ifsc");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqBankDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_bank_det_tbl_enq_temptab");
            });

            modelBuilder.Entity<TblEnqBasicDet>(entity =>
            {
                entity.HasKey(e => e.EnqBdetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_basic_det");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_enq_basic_det_OFFC_CDTAB");

                entity.HasIndex(e => e.VilCd, "fk_tbl_enq_basic_det_VIL_CDTAB");

                entity.HasIndex(e => e.ConstCd, "fk_tbl_enq_basic_det_tbl_cnst_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_basic_det_tbl_enq_temptab_idx");

                entity.HasIndex(e => e.PremCd, "fk_tbl_enq_basic_det_tbl_prem_cdtab");

                entity.HasIndex(e => e.PurpCd, "fk_tbl_enq_basic_det_tbl_purp_cdtab");

                entity.HasIndex(e => e.SizeCd, "fk_tbl_enq_basic_det_tbl_size_cdtab");

                entity.Property(e => e.EnqBdetId).HasColumnName("enq_bdet_id");

                entity.Property(e => e.AddlLoan).HasColumnName("addl_loan");

                entity.Property(e => e.ConstCd).HasColumnName("const_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqAddress)
                    .HasMaxLength(200)
                    .HasColumnName("enq_address");

                entity.Property(e => e.EnqApplName)
                    .HasMaxLength(100)
                    .HasColumnName("enq_appl_name");

                entity.Property(e => e.EnqEmail)
                    .HasMaxLength(50)
                    .HasColumnName("enq_email");

                entity.Property(e => e.EnqLoanamt).HasColumnName("enq_loanamt");

                entity.Property(e => e.EnqPincode).HasColumnName("enq_pincode");

                entity.Property(e => e.EnqPlace)
                    .HasMaxLength(100)
                    .HasColumnName("enq_place");

                entity.Property(e => e.EnqRepayPeriod).HasColumnName("enq_repay_period");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("offc_cd");

                entity.Property(e => e.PremCd).HasColumnName("prem_cd");

                entity.Property(e => e.ProdCd).HasColumnName("prod_cd");

                entity.Property(e => e.PurpCd).HasColumnName("purp_cd");

                entity.Property(e => e.SizeCd).HasColumnName("size_cd");
                entity.Property(e => e.IndCd).HasColumnName("ind_cd");
                entity.Property(e => e.PromotorClass).HasColumnName("prom_class");
                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UnitName)
                    .HasMaxLength(150)
                    .HasColumnName("unit_name");

                entity.Property(e => e.VilCd).HasColumnName("vil_cd");

                entity.HasOne(d => d.ConstCdNavigation)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.ConstCd)
                    .HasConstraintName("fk_tbl_enq_basic_det_tbl_cnst_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_basic_det_tbl_enq_temptab");

                entity.HasOne(d => d.OffcCdNavigation)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.OffcCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_basic_det_OFFC_CDTAB");

                entity.HasOne(d => d.PremCdNavigation)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.PremCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_basic_det_tbl_prem_cdtab");

                entity.HasOne(d => d.PurpCdNavigation)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.PurpCd)
                    .HasConstraintName("fk_tbl_enq_basic_det_tbl_purp_cdtab");

                entity.HasOne(d => d.SizeCdNavigation)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.SizeCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_basic_det_tbl_size_cdtab");

                entity.HasOne(d => d.VilCdNavigation)
                    .WithMany(p => p.TblEnqBasicDets)
                    .HasForeignKey(d => d.VilCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_basic_det_VIL_CDTAB");
            });

            modelBuilder.Entity<TblEnqDocDet>(entity =>
            {
                entity.HasKey(e => e.EnqDocId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_doc_det");

                entity.HasIndex(e => e.DoccatCd, "fk_tbl_enq_doc_det_tbl_doccat_cdtab");

                entity.HasIndex(e => e.DocdetCd, "fk_tbl_enq_doc_det_tbl_docdetails_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_doc_det_tbl_enq_temptab");

                entity.Property(e => e.EnqDocId).HasColumnName("enq_doc_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DoccatCd).HasColumnName("doccat_cd");

                entity.Property(e => e.DocdetCd).HasColumnName("docdet_cd");

                entity.Property(e => e.EnqDocPath)
                    .HasMaxLength(200)
                    .HasColumnName("enq_doc_path");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DoccatCdNavigation)
                    .WithMany(p => p.TblEnqDocDets)
                    .HasForeignKey(d => d.DoccatCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_doc_det_tbl_doccat_cdtab");

                entity.HasOne(d => d.DocdetCdNavigation)
                    .WithMany(p => p.TblEnqDocDets)
                    .HasForeignKey(d => d.DocdetCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_doc_det_tbl_docdetails_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqDocDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_doc_det_tbl_enq_temptab");
            });

            modelBuilder.Entity<TblEnqDocument>(entity =>
            {
                entity.ToTable("tbl_enq_document");

                entity.HasIndex(e => e.Id, "id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(45)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.Description).HasMaxLength(450);

                entity.Property(e => e.DocSection).HasMaxLength(45);

                entity.Property(e => e.FilePath).HasMaxLength(200);

                entity.Property(e => e.FileType).HasMaxLength(45);

                entity.Property(e => e.IsActive)
                    .HasColumnType("tinyint")
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnType("tinyint")
                    .HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(45)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.Name).HasMaxLength(150);

                entity.Property(e => e.Process).HasMaxLength(45);

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(150)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblEnqGassetDet>(entity =>
            {
                entity.HasKey(e => e.EnqGuarassetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_gasset_det");

                entity.HasIndex(e => e.AssetcatCd, "fk_tbl_enq_gasset_det_tbl_assetcat_cdtab");

                entity.HasIndex(e => e.AssettypeCd, "fk_tbl_enq_gasset_det_tbl_assettype_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_gasset_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_gasset_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqGuarassetId).HasColumnName("enq_guarasset_id");

                entity.Property(e => e.AssetcatCd).HasColumnName("assetcat_cd");

                entity.Property(e => e.AssettypeCd).HasColumnName("assettype_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.GuarAssetAddr)
                    .HasMaxLength(200)
                    .HasColumnName("guar_asset_addr");

                entity.Property(e => e.GuarAssetArea)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_asset_area");

                entity.Property(e => e.GuarAssetDesc)
                    .HasMaxLength(500)
                    .HasColumnName("guar_asset_desc");

                entity.Property(e => e.GuarAssetDim)
                    .HasMaxLength(100)
                    .HasColumnName("guar_asset_dim");

                entity.Property(e => e.GuarAssetSiteno)
                    .HasMaxLength(50)
                    .HasColumnName("guar_asset_siteno");

                entity.Property(e => e.GuarAssetValue)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_asset_value");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.AssetcatCdNavigation)
                    .WithMany(p => p.TblEnqGassetDets)
                    .HasForeignKey(d => d.AssetcatCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_gasset_det_tbl_assetcat_cdtab");

                entity.HasOne(d => d.AssettypeCdNavigation)
                    .WithMany(p => p.TblEnqGassetDets)
                    .HasForeignKey(d => d.AssettypeCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_gasset_det_tbl_assettype_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqGassetDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_gasset_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqGassetDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_gasset_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqGbankDet>(entity =>
            {
                entity.HasKey(e => e.EnqGuarbankId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_gbank_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_gbank_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_gbank_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqGuarbankId).HasColumnName("enq_guarbank_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.GuarAccName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("guar_acc_name");

                entity.Property(e => e.GuarAcctype)
                    .HasMaxLength(20)
                    .HasColumnName("guar_acctype");

                entity.Property(e => e.GuarBankaccno)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("guar_bankaccno");

                entity.Property(e => e.GuarBankbr)
                    .HasMaxLength(100)
                    .HasColumnName("guar_bankbr");

                entity.Property(e => e.GuarBankname)
                    .HasMaxLength(100)
                    .HasColumnName("guar_bankname");

                entity.Property(e => e.GuarIfsc)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("guar_ifsc");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqGbankDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_gbank_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqGbankDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_gbank_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqGliabDet>(entity =>
            {
                entity.HasKey(e => e.EnqGuarliabId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_gliab_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_gliab_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_gliab_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqGuarliabId).HasColumnName("enq_guarliab_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.GuarLiabDesc)
                    .HasMaxLength(500)
                    .HasColumnName("guar_liab_desc");

                entity.Property(e => e.GuarLiabValue)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_liab_value");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqGliabDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_gliab_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqGliabDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_gliab_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqGnwDet>(entity =>
            {
                entity.HasKey(e => e.EnqGuarnwId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_gnw_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_gnw_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_gnw_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqGuarnwId).HasColumnName("enq_guarnw_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.GuarImmov)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_immov");

                entity.Property(e => e.GuarLiab)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_liab");

                entity.Property(e => e.GuarMov)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_mov");

                entity.Property(e => e.GuarNw)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("guar_nw");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqGnwDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_gnw_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqGnwDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_gnw_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqGuarDet>(entity =>
            {
                entity.HasKey(e => e.EnqGuarId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_guar_det");

                entity.HasIndex(e => e.DomCd, "fk_tbl_enq_guar_det_tbl_domi_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_guar_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_guar_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqGuarId).HasColumnName("enq_guar_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DomCd).HasColumnName("dom_cd");

                entity.Property(e => e.EnqGuarcibil).HasColumnName("enq_guarcibil");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DomCdNavigation)
                    .WithMany(p => p.TblEnqGuarDets)
                    .HasForeignKey(d => d.DomCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_guar_det_tbl_domi_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqGuarDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_guar_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqGuarDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_guar_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqMftotalDet>(entity =>
            {
                entity.HasKey(e => e.EnqMftotalId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_mftotal_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_mftotal_det_tbl_enq_temptab");

                entity.Property(e => e.EnqMftotalId).HasColumnName("enq_mftotal_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqDebt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_debt");

                entity.Property(e => e.EnqEquity)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_equity");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqMftotalDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_mftotal_det_tbl_enq_temptab");
            });

            modelBuilder.Entity<TblEnqPassetDet>(entity =>
            {
                entity.HasKey(e => e.EnqPromassetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_passet_det");

                entity.HasIndex(e => e.AssetcatCd, "fk_tbl_enq_passet_det_tbl_assetcat_cdtab");

                entity.HasIndex(e => e.AssettypeCd, "fk_tbl_enq_passet_det_tbl_assettype_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_passet_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_passet_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqPromassetId).HasColumnName("enq_promasset_id");

                entity.Property(e => e.AssetcatCd).HasColumnName("assetcat_cd");

                entity.Property(e => e.AssettypeCd).HasColumnName("assettype_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqAssetAddr)
                    .HasMaxLength(200)
                    .HasColumnName("enq_asset_addr");

                entity.Property(e => e.EnqAssetArea)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_asset_area");

                entity.Property(e => e.EnqAssetDesc)
                    .HasMaxLength(500)
                    .HasColumnName("enq_asset_desc");

                entity.Property(e => e.EnqAssetDim)
                    .HasMaxLength(100)
                    .HasColumnName("enq_asset_dim");

                entity.Property(e => e.EnqAssetSiteno)
                    .HasMaxLength(50)
                    .HasColumnName("enq_asset_siteno");

                entity.Property(e => e.EnqAssetValue)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_asset_value");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.AssetcatCdNavigation)
                    .WithMany(p => p.TblEnqPassetDets)
                    .HasForeignKey(d => d.AssetcatCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_passet_det_tbl_assetcat_cdtab");

                entity.HasOne(d => d.AssettypeCdNavigation)
                    .WithMany(p => p.TblEnqPassetDets)
                    .HasForeignKey(d => d.AssettypeCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_passet_det_tbl_assettype_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPassetDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_passet_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqPassetDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_passet_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqPbankDet>(entity =>
            {
                entity.HasKey(e => e.EnqPrombankId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_pbank_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_pbank_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_pbank_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqPrombankId).HasColumnName("enq_prombank_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromAccName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("prom_acc_name");

                entity.Property(e => e.PromAcctype)
                    .HasMaxLength(20)
                    .HasColumnName("prom_acctype");

                entity.Property(e => e.PromBankaccno)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("prom_bankaccno");

                entity.Property(e => e.PromBankbr)
                    .HasMaxLength(100)
                    .HasColumnName("prom_bankbr");

                entity.Property(e => e.PromBankname)
                    .HasMaxLength(100)
                    .HasColumnName("prom_bankname");

                entity.Property(e => e.PromIfsc)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("prom_ifsc");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPbankDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_pbank_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqPbankDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pbank_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqPjcostDet>(entity =>
            {
                entity.HasKey(e => e.EnqPjcostId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_pjcost_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_pjcost_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PjcostCd, "fk_tbl_enq_pjcost_det_tbl_pjcost_cdtab");

                entity.Property(e => e.EnqPjcostId).HasColumnName("enq_pjcost_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqPjcostAmt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_pjcost_amt");

                entity.Property(e => e.EnqPjcostRem)
                    .HasMaxLength(100)
                    .HasColumnName("enq_pjcost_rem");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjcostCd).HasColumnName("pjcost_cd");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPjcostDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pjcost_det_tbl_enq_temptab");

                entity.HasOne(d => d.PjcostCdNavigation)
                    .WithMany(p => p.TblEnqPjcostDets)
                    .HasForeignKey(d => d.PjcostCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pjcost_det_tbl_pjcost_cdtab");
            });

            modelBuilder.Entity<TblEnqPjfinDet>(entity =>
            {
                entity.HasKey(e => e.EnqPjfinId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_pjfin_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_pjfin_det_tbl_enq_temptab");

                entity.HasIndex(e => e.FincompCd, "fk_tbl_enq_pjfin_det_tbl_fincomp_cdtab");

                entity.HasIndex(e => e.FinyearCode, "fk_tbl_enq_pjfin_det_tbl_finyear_cdtab");

                entity.Property(e => e.EnqPjfinId).HasColumnName("enq_pjfin_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqPjfinamt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_pjfinamt");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.FincompCd).HasColumnName("fincomp_cd");

                entity.Property(e => e.FinyearCode)
                    .HasColumnType("mediumint")
                    .HasColumnName("finyear_code");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.WhPjprov).HasColumnName("wh_pjprov");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPjfinDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_pjfin_det_tbl_enq_temptab");

                entity.HasOne(d => d.FincompCdNavigation)
                    .WithMany(p => p.TblEnqPjfinDets)
                    .HasForeignKey(d => d.FincompCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pjfin_det_tbl_fincomp_cdtab");

                entity.HasOne(d => d.FinyearCodeNavigation)
                    .WithMany(p => p.TblEnqPjfinDets)
                    .HasForeignKey(d => d.FinyearCode)
                    .HasConstraintName("fk_tbl_enq_pjfin_det_tbl_finyear_cdtab");
            });

            modelBuilder.Entity<TblEnqPjmfDet>(entity =>
            {
                entity.HasKey(e => e.EnqPjmfId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_pjmf_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_pjmf_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PjmfCd, "fk_tbl_enq_pjmf_det_tbl_pjmf_cdtab");

                entity.HasIndex(e => e.MfcatCd, "fk_tbl_enq_pjmf_det_tbl_pjmfcat_cdtab_idx");

                entity.Property(e => e.EnqPjmfId).HasColumnName("enq_pjmf_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqPjmfValue)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_pjmf_value");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.MfcatCd).HasColumnName("mfcat_cd");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjmfCd).HasColumnName("pjmf_cd");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPjmfDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_pjmf_det_tbl_enq_temptab");

                entity.HasOne(d => d.MfcatCdNavigation)
                    .WithMany(p => p.TblEnqPjmfDets)
                    .HasForeignKey(d => d.MfcatCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pjmf_det_tbl_pjmfcat_cdtab");

                entity.HasOne(d => d.PjmfCdNavigation)
                    .WithMany(p => p.TblEnqPjmfDets)
                    .HasForeignKey(d => d.PjmfCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pjmf_det_tbl_pjmf_cdtab");
            });

            modelBuilder.Entity<TblEnqPliabDet>(entity =>
            {
                entity.HasKey(e => e.EnqPromliabId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_pliab_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_pliab_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_pliab_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqPromliabId).HasColumnName("enq_promliab_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqLiabDesc)
                    .HasMaxLength(500)
                    .HasColumnName("enq_liab_desc");

                entity.Property(e => e.EnqLiabValue)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_liab_value");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPliabDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_pliab_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqPliabDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pliab_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqPnwDet>(entity =>
            {
                entity.HasKey(e => e.EnqPromnwId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_pnw_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_pnw_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_pnw_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqPromnwId).HasColumnName("enq_promnw_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqImmov)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_immov");

                entity.Property(e => e.EnqLiab)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_liab");

                entity.Property(e => e.EnqMov)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_mov");

                entity.Property(e => e.EnqNw)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_nw");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPnwDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_pnw_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqPnwDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_pnw_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqPromDet>(entity =>
            {
                entity.HasKey(e => e.EnqPromId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_prom_det");

                entity.HasIndex(e => e.DomCd, "fk_tbl_enq_prom_det_tbl_domi_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_prom_det_tbl_enq_temptab");

                entity.HasIndex(e => e.PdesigCd, "fk_tbl_enq_prom_det_tbl_pdesig_cdtab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_enq_prom_det_tbl_prom_cdtab");

                entity.Property(e => e.EnqPromId).HasColumnName("enq_prom_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DomCd).HasColumnName("dom_cd");

                entity.Property(e => e.EnqCibil).HasColumnName("enq_cibil");

                entity.Property(e => e.EnqPromExp)
                    .HasColumnType("mediumint")
                    .HasColumnName("enq_prom_exp");

                entity.Property(e => e.EnqPromExpdet)
                    .HasMaxLength(500)
                    .HasColumnName("enq_prom_expdet");

                entity.Property(e => e.EnqPromShare)
                    .HasColumnType("decimal(3,2)")
                    .HasColumnName("enq_prom_share");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PdesigCd).HasColumnName("pdesig_cd");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DomCdNavigation)
                    .WithMany(p => p.TblEnqPromDets)
                    .HasForeignKey(d => d.DomCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_prom_det_tbl_domi_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqPromDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_prom_det_tbl_enq_temptab");

                entity.HasOne(d => d.PdesigCdNavigation)
                    .WithMany(p => p.TblEnqPromDets)
                    .HasForeignKey(d => d.PdesigCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_prom_det_tbl_pdesig_cdtab");

                entity.HasOne(d => d.PromoterCodeNavigation)
                    .WithMany(p => p.TblEnqPromDets)
                    .HasForeignKey(d => d.PromoterCode)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_prom_det_tbl_prom_cdtab");
            });

            modelBuilder.Entity<TblEnqRegnoDet>(entity =>
            {
                entity.HasKey(e => e.EnqRegnoId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_regno_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_regno_det_tbl_enq_temptab");

                entity.HasIndex(e => e.RegrefCd, "fk_tbl_enq_regno_det_tbl_regdetails_cdtab");

                entity.Property(e => e.EnqRegnoId).HasColumnName("enq_regno_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqRegno)
                    .HasMaxLength(50)
                    .HasColumnName("enq_regno");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.RegrefCd).HasColumnName("regref_cd");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqRegnoDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_regno_det_tbl_enq_temptab");

                entity.HasOne(d => d.RegrefCdNavigation)
                    .WithMany(p => p.TblEnqRegnoDets)
                    .HasForeignKey(d => d.RegrefCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_regno_det_tbl_regdetails_cdtab");
            });

            modelBuilder.Entity<TblEnqSecDet>(entity =>
            {
                entity.HasKey(e => e.EnqSecId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_sec_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_sec_det_tbl_enq_temptab");

                entity.HasIndex(e => e.SecCode, "fk_tbl_enq_sec_det_tbl_pjsec_cdtab");

                entity.HasIndex(e => e.PromrelCd, "fk_tbl_enq_sec_det_tbl_promrel_cdtab");

                entity.HasIndex(e => e.SecCd, "fk_tbl_enq_sec_det_tbl_sec_cdtab");

                entity.Property(e => e.EnqSecId).HasColumnName("enq_sec_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqSecDesc)
                    .HasMaxLength(300)
                    .HasColumnName("enq_sec_desc");

                entity.Property(e => e.EnqSecName)
                    .HasMaxLength(100)
                    .HasColumnName("enq_sec_name");

                entity.Property(e => e.EnqSecValue)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_sec_value");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromrelCd).HasColumnName("promrel_cd");

                entity.Property(e => e.SecCd).HasColumnName("sec_cd");

                entity.Property(e => e.SecCode)
                    .HasColumnType("mediumint")
                    .HasColumnName("sec_code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqSecDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_sec_det_tbl_enq_temptab");

                entity.HasOne(d => d.PromrelCdNavigation)
                    .WithMany(p => p.TblEnqSecDets)
                    .HasForeignKey(d => d.PromrelCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_sec_det_tbl_promrel_cdtab");

                entity.HasOne(d => d.SecCdNavigation)
                    .WithMany(p => p.TblEnqSecDets)
                    .HasForeignKey(d => d.SecCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_sec_det_tbl_sec_cdtab");

                entity.HasOne(d => d.SecCodeNavigation)
                    .WithMany(p => p.TblEnqSecDets)
                    .HasForeignKey(d => d.SecCode)
                    .HasConstraintName("fk_tbl_enq_sec_det_tbl_pjsec_cdtab");
            });

            modelBuilder.Entity<TblEnqSfinDet>(entity =>
            {
                entity.HasKey(e => e.EnqSisfinId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_sfin_det");

                entity.HasIndex(e => e.EnqSisId, "fk_tbl_enq_sfin_det_tbl_enq_sis_det");

                entity.HasIndex(e => e.FincompCd, "fk_tbl_enq_sfin_det_tbl_fincomp_cdtab");

                entity.HasIndex(e => e.FinyearCode, "fk_tbl_enq_sfin_det_tbl_finyear_cdtab");

                entity.Property(e => e.EnqSisfinId).HasColumnName("enq_sisfin_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqFinamt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_finamt");

                entity.Property(e => e.EnqSisId).HasColumnName("enq_sis_id");

                entity.Property(e => e.FincompCd).HasColumnName("fincomp_cd");

                entity.Property(e => e.FinyearCode)
                    .HasColumnType("mediumint")
                    .HasColumnName("finyear_code");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.WhProv)
                    .HasColumnType("bit(1)")
                    .HasColumnName("wh_prov");

                entity.HasOne(d => d.EnqSis)
                    .WithMany(p => p.TblEnqSfinDets)
                    .HasForeignKey(d => d.EnqSisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_sfin_det_tbl_enq_sis_det");

                entity.HasOne(d => d.FincompCdNavigation)
                    .WithMany(p => p.TblEnqSfinDets)
                    .HasForeignKey(d => d.FincompCd)
                    .HasConstraintName("fk_tbl_enq_sfin_det_tbl_fincomp_cdtab");

                entity.HasOne(d => d.FinyearCodeNavigation)
                    .WithMany(p => p.TblEnqSfinDets)
                    .HasForeignKey(d => d.FinyearCode)
                    .HasConstraintName("fk_tbl_enq_sfin_det_tbl_finyear_cdtab");
            });

            modelBuilder.Entity<TblEnqSisDet>(entity =>
            {
                entity.HasKey(e => e.EnqSisId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_sis_det");

                entity.HasIndex(e => e.BfacilityCode, "fk_tbl_enq_sis_det_tbl_bankfacility_cdtab");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_sis_det_tbl_enq_temptab");

                entity.Property(e => e.EnqSisId).HasColumnName("enq_sis_id");

                entity.Property(e => e.BfacilityCode).HasColumnName("bfacility_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqDeftamt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_deftamt");

                entity.Property(e => e.EnqOts)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_ots");

                entity.Property(e => e.EnqOutamt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_outamt");

                entity.Property(e => e.EnqRelief)
                    .HasMaxLength(200)
                    .HasColumnName("enq_relief");

                entity.Property(e => e.EnqSisIfsc)
                    .HasMaxLength(11)
                    .HasColumnName("enq_sis_ifsc");

                entity.Property(e => e.EnqSisName)
                    .HasMaxLength(100)
                    .HasColumnName("enq_sis_name");

                entity.Property(e => e.EnqSiscibil).HasColumnName("enq_siscibil");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.BfacilityCodeNavigation)
                    .WithMany(p => p.TblEnqSisDets)
                    .HasForeignKey(d => d.BfacilityCode)
                    .HasConstraintName("fk_tbl_enq_sis_det_tbl_bankfacility_cdtab");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqSisDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_sis_det_tbl_enq_temptab");
            });

            modelBuilder.Entity<TblEnqTemptab>(entity =>
            {
                entity.HasKey(e => e.EnqtempId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_temptab");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqInitDate)
                    .HasColumnType("date")
                    .HasColumnName("enq_init_date");

                entity.Property(e => e.EnqNote)
                    .HasMaxLength(1000)
                    .HasColumnName("enq_note");

                entity.Property(e => e.EnqRefNo).HasColumnName("enq_ref_no");

                entity.Property(e => e.EnqStatus).HasColumnName("enq_status");
                entity.Property(e => e.HasAssociateSisterConcern).HasColumnName("has_associate_sisterconcern");

                entity.Property(e => e.EnqSubmitDate)
                    .HasColumnType("date")
                    .HasColumnName("enq_submit_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromPan)
                    .IsRequired()
                    .HasMaxLength(11)
                    .HasColumnName("prom_pan");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblEnqTrcostDet>(entity =>
            {
                entity.HasKey(e => e.EnqTrjcostId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_trcost_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_trcost_det_tbl_enq_temptab");

                entity.HasIndex(e => e.TrPjcostCd, "fk_tbl_enq_trcost_det_tbl_tr_pjcost_cdtab");

                entity.Property(e => e.EnqTrjcostId).HasColumnName("enq_trjcost_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqPjcostAmt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_pjcost_amt");

                entity.Property(e => e.EnqPjcostRem)
                    .HasMaxLength(100)
                    .HasColumnName("enq_pjcost_rem");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.TrPjcostCd).HasColumnName("tr_pjcost_cd");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqTrcostDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_trcost_det_tbl_enq_temptab");

                entity.HasOne(d => d.TrPjcostCdNavigation)
                    .WithMany(p => p.TblEnqTrcostDets)
                    .HasForeignKey(d => d.TrPjcostCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_enq_trcost_det_tbl_tr_pjcost_cdtab");
            });

            modelBuilder.Entity<TblEnqWcDet>(entity =>
            {
                entity.HasKey(e => e.EnqWcId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_enq_wc_det");

                entity.HasIndex(e => e.EnqtempId, "fk_tbl_enq_wc_det_tbl_enq_temptab");

                entity.Property(e => e.EnqWcId).HasColumnName("enq_wc_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqWcAmt)
                    .HasColumnType("decimal(10,1)")
                    .HasColumnName("enq_wc_amt");

                entity.Property(e => e.EnqWcBank)
                    .HasMaxLength(100)
                    .HasColumnName("enq_wc_bank");

                entity.Property(e => e.EnqWcBranch)
                    .HasMaxLength(100)
                    .HasColumnName("enq_wc_branch");

                entity.Property(e => e.EnqWcIfsc)
                    .HasMaxLength(11)
                    .HasColumnName("enq_wc_ifsc");

                entity.Property(e => e.EnqtempId).HasColumnName("enqtemp_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Enqtemp)
                    .WithMany(p => p.TblEnqWcDets)
                    .HasForeignKey(d => d.EnqtempId)
                    .HasConstraintName("fk_tbl_enq_wc_det_tbl_enq_temptab");
            });

            modelBuilder.Entity<TblFincompCdtab>(entity =>
            {
                entity.HasKey(e => e.FincompCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_fincomp_cdtab");

                entity.Property(e => e.FincompCd).HasColumnName("fincomp_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.FincompDets)
                    .HasMaxLength(100)
                    .HasColumnName("fincomp_dets");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SeqNo)
                    .HasColumnType("decimal(3,2)")
                    .HasColumnName("seq_no");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblFinyearCdtab>(entity =>
            {
                entity.HasKey(e => e.FinyearCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_finyear_cdtab");

                entity.Property(e => e.FinyearCode)
                    .HasColumnType("mediumint")
                    .HasColumnName("finyear_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.FinyearDesc)
                    .HasMaxLength(9)
                    .HasColumnName("finyear_desc");

                entity.Property(e => e.FromDate)
                    .HasColumnType("date")
                    .HasColumnName("from_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ToDate)
                    .HasColumnType("date")
                    .HasColumnName("to_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblIndCdtab>(entity =>
            {
                entity.HasKey(e => e.IndCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_ind_cdtab");

                entity.Property(e => e.IndCd).HasColumnName("ind_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IndDets)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("ind_dets");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblModuleCdtab>(entity =>
            {
                entity.HasKey(e => e.ModuleId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_module_cdtab");

                entity.Property(e => e.ModuleId).HasColumnName("module_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ModuleDesc)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("module_desc");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblNwCdtab>(entity =>
            {
                entity.ToTable("tbl_nw_cdtab");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.NetCd).HasColumnName("net_cd");

                entity.Property(e => e.NetDesc)
                    .HasMaxLength(65)
                    .HasColumnName("net_desc");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPclasCdtab>(entity =>
            {
                entity.ToTable("tbl_pclas_cdtab");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.PclasCd).HasColumnName("pclas_cd");

                entity.Property(e => e.PclasDets)
                    .HasMaxLength(30)
                    .HasColumnName("pclas_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPdesigCdtab>(entity =>
            {
                entity.HasKey(e => e.PdesigCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pdesig_cdtab");

                entity.Property(e => e.PdesigCd).HasColumnName("pdesig_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PdesigDets)
                    .HasMaxLength(100)
                    .HasColumnName("pdesig_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPjcostCdtab>(entity =>
            {
                entity.HasKey(e => e.PjcostCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pjcost_cdtab");

                entity.HasIndex(e => e.PjcostgroupCd, "fk_tbl_pjcost_cdtab_tbl_pjcostgroup_cdtab");

                entity.HasIndex(e => e.PjcsgroupCd, "fk_tbl_pjcost_cdtab_tbl_pjcsgroup_cdtab");

                entity.Property(e => e.PjcostCd).HasColumnName("pjcost_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjcostDets)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("pjcost_dets");

                entity.Property(e => e.PjcostFlg).HasColumnName("pjcost_flg");

                entity.Property(e => e.PjcostgroupCd).HasColumnName("pjcostgroup_cd");

                entity.Property(e => e.PjcsgroupCd).HasColumnName("pjcsgroup_cd");

                entity.Property(e => e.SeqNo)
                    .HasColumnType("decimal(3,2)")
                    .HasColumnName("seq_no");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.PjcostgroupCdNavigation)
                    .WithMany(p => p.TblPjcostCdtabs)
                    .HasForeignKey(d => d.PjcostgroupCd)
                    .HasConstraintName("fk_tbl_pjcost_cdtab_tbl_pjcostgroup_cdtab");

                entity.HasOne(d => d.PjcsgroupCdNavigation)
                    .WithMany(p => p.TblPjcostCdtabs)
                    .HasForeignKey(d => d.PjcsgroupCd)
                    .HasConstraintName("fk_tbl_pjcost_cdtab_tbl_pjcsgroup_cdtab");
            });

            modelBuilder.Entity<TblPjcostgroupCdtab>(entity =>
            {
                entity.HasKey(e => e.PjcostgroupCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pjcostgroup_cdtab");

                entity.Property(e => e.PjcostgroupCd).HasColumnName("pjcostgroup_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjcostgroupDets)
                    .HasMaxLength(100)
                    .HasColumnName("pjcostgroup_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPjcsgroupCdtab>(entity =>
            {
                entity.HasKey(e => e.PjcsgroupCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pjcsgroup_cdtab");

                entity.HasIndex(e => e.PjcostgroupCd, "fk_tbl_pjcsgroup_cdtab_tbl_pjcostgroup_cdtab");

                entity.Property(e => e.PjcsgroupCd).HasColumnName("pjcsgroup_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjcostgroupCd).HasColumnName("pjcostgroup_cd");

                entity.Property(e => e.PjcsgroupDets)
                    .HasMaxLength(100)
                    .HasColumnName("pjcsgroup_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.PjcostgroupCdNavigation)
                    .WithMany(p => p.TblPjcsgroupCdtabs)
                    .HasForeignKey(d => d.PjcostgroupCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_pjcsgroup_cdtab_tbl_pjcostgroup_cdtab");
            });

            modelBuilder.Entity<TblPjmfCdtab>(entity =>
            {
                entity.HasKey(e => e.PjmfCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pjmf_cdtab");

                entity.HasIndex(e => e.MfcatCd, "fk_tbl_pjmf_cdtab_tbl_pjmfcat_cdtab");

                entity.Property(e => e.PjmfCd).HasColumnName("pjmf_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.MfcatCd).HasColumnName("mfcat_cd");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjmfDets)
                    .IsRequired()
                    .HasMaxLength(55)
                    .HasColumnName("pjmf_dets");

                entity.Property(e => e.PjmfFlg)
                    .HasColumnType("tinyint")
                    .HasColumnName("pjmf_flg");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.MfcatCdNavigation)
                    .WithMany(p => p.TblPjmfCdtabs)
                    .HasForeignKey(d => d.MfcatCd)
                    .HasConstraintName("fk_tbl_pjmf_cdtab_tbl_pjmfcat_cdtab");
            });

            modelBuilder.Entity<TblPjmfcatCdtab>(entity =>
            {
                entity.HasKey(e => e.MfcatCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pjmfcat_cdtab");

                entity.Property(e => e.MfcatCd).HasColumnName("mfcat_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PjmfDets)
                    .HasMaxLength(100)
                    .HasColumnName("pjmf_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPjsecCdtab>(entity =>
            {
                entity.HasKey(e => e.SecCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pjsec_cdtab");

                entity.Property(e => e.SecCode)
                    .HasColumnType("mediumint")
                    .HasColumnName("sec_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SecDets)
                    .HasMaxLength(50)
                    .HasColumnName("sec_dets");

                entity.Property(e => e.SecFlg)
                    .HasColumnType("tinyint")
                    .HasColumnName("sec_flg");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPremCdtab>(entity =>
            {
                entity.HasKey(e => e.PremCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_prem_cdtab");

                entity.Property(e => e.PremCd).HasColumnName("prem_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PremDets)
                    .HasMaxLength(30)
                    .HasColumnName("prem_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblProdCdtab>(entity =>
            {
                entity.ToTable("tbl_prod_cdtab");

                entity.HasIndex(e => e.ProdInd, "fk_tbl_prod_cdtab_tbl_ind_cdtab");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.Dept)
                    .HasMaxLength(6)
                    .HasColumnName("dept");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ProdCd).HasColumnName("prod_cd");

                entity.Property(e => e.ProdDets)
                    .IsRequired()
                    .HasMaxLength(80)
                    .HasColumnName("prod_dets");

                entity.Property(e => e.ProdInd).HasColumnName("prod_ind");

                entity.Property(e => e.ProdNcd).HasColumnName("prod_ncd");

                entity.Property(e => e.ProdNdt)
                    .HasMaxLength(50)
                    .HasColumnName("prod_ndt");

                entity.Property(e => e.ProfFlg).HasColumnName("prof_flg");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.ProdIndNavigation)
                    .WithMany(p => p.TblProdCdtabs)
                    .HasForeignKey(d => d.ProdInd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_prod_cdtab_tbl_ind_cdtab");
            });

            modelBuilder.Entity<TblPromCdtab>(entity =>
            {
                entity.HasKey(e => e.PromoterCode)
                    .HasName("PRIMARY");


                entity.ToTable("tbl_prom_cdtab");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PanValidationDate)
                    .HasColumnType("date")
                    .HasColumnName("pan_validation_date");

                entity.Property(e => e.PromoterDob)
                    .HasColumnType("date")
                    .HasColumnName("promoter_dob");

                entity.Property(e => e.PromoterEmailid)
                    .HasMaxLength(50)
                    .HasColumnName("promoter_emailid");

                entity.Property(e => e.PromoterGender)
                    .IsRequired()
                    .HasColumnType("tinytext")
                    .HasColumnName("promoter_gender");

                entity.Property(e => e.PromoterMobno).HasColumnName("promoter_mobno");

                entity.Property(e => e.PromoterName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("promoter_name");

                entity.Property(e => e.PromoterPan)
                    .HasMaxLength(11)
                    .HasColumnName("promoter_pan");

                entity.Property(e => e.PromoterPassport)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("promoter_passport");

                entity.Property(e => e.PromoterPhoto)
                    .HasMaxLength(300)
                    .HasColumnName("promoter_photo");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

            });

            modelBuilder.Entity<TblPromUid>(entity =>
            {
                entity.HasKey(e => e.PromoterCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_prom_uid");

                entity.Property(e => e.PromoterCode).HasColumnName("promoter_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromUid).HasColumnName("prom_uid");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPromrelCdtab>(entity =>
            {
                entity.HasKey(e => e.PromrelCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_promrel_cdtab");

                entity.Property(e => e.PromrelCd).HasColumnName("promrel_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PromrelDets)
                    .HasMaxLength(100)
                    .HasColumnName("promrel_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblPurpCdtab>(entity =>
            {
                entity.HasKey(e => e.PurpCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_purp_cdtab");

                entity.Property(e => e.PurpCd).HasColumnName("purp_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PurpDets)
                    .HasMaxLength(50)
                    .HasColumnName("purp_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblRegdetailsCdtab>(entity =>
            {
                entity.HasKey(e => e.RegrefCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_regdetails_cdtab");

                entity.Property(e => e.RegrefCd).HasColumnName("regref_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.RegrefDets)
                    .HasMaxLength(50)
                    .HasColumnName("regref_dets");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblRoleCdtab>(entity =>
            {
                entity.HasKey(e => e.RoleId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_role_cdtab");

                entity.HasIndex(e => e.ModuleId, "fk_tbl_role_cdtab_tbl_module_cdtab");

                entity.Property(e => e.RoleId).HasColumnName("role_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ModuleId).HasColumnName("module_id");

                entity.Property(e => e.RoleDesc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("role_desc");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.Module)
                    .WithMany(p => p.TblRoleCdtabs)
                    .HasForeignKey(d => d.ModuleId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_role_cdtab_tbl_module_cdtab");
            });

            modelBuilder.Entity<TblSecCdtab>(entity =>
            {
                entity.HasKey(e => e.SecCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_sec_cdtab");

                entity.Property(e => e.SecCd).HasColumnName("sec_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SecDets)
                    .HasMaxLength(150)
                    .HasColumnName("sec_dets");

                entity.Property(e => e.SecTy)
                    .HasColumnType("tinyint")
                    .HasColumnName("sec_ty");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblSecProdMast>(entity =>
            {
                entity.HasKey(e => e.SecCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_sec_prod_mast");

                entity.Property(e => e.SecCd).HasColumnName("sec_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SecDesc)
                    .HasMaxLength(80)
                    .HasColumnName("sec_desc");

                entity.Property(e => e.SecProd).HasColumnName("sec_prod");

                entity.Property(e => e.SecSector)
                    .HasMaxLength(50)
                    .HasColumnName("sec_sector");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblSizeCdtab>(entity =>
            {
                entity.HasKey(e => e.SizeCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_size_cdtab");

                entity.Property(e => e.SizeCd).HasColumnName("size_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SizeDets)
                    .IsRequired()
                    .HasMaxLength(50)
                    .HasColumnName("size_dets");

                entity.Property(e => e.SizeFlag).HasColumnName("size_flag");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblSubattrCdtab>(entity =>
            {
                entity.HasKey(e => e.SubattrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_subattr_cdtab");

                entity.HasIndex(e => e.AttrId, "fk_tbl_subattr_cdtab_tbl_attr_cdtab");

                entity.HasIndex(e => e.AttrunitId, "fk_tbl_subattr_cdtab_tbl_attrunit_cdtab");

                entity.HasIndex(e => e.UnitoptrId, "fk_tbl_subattr_cdtab_tbl_unitoptr_cdtab");

                entity.Property(e => e.SubattrId).HasColumnName("subattr_id");

                entity.Property(e => e.AttrId).HasColumnName("attr_id");

                entity.Property(e => e.AttrunitId).HasColumnName("attrunit_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SubattrDesc)
                    .HasMaxLength(100)
                    .HasColumnName("subattr_desc");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UnitoptrId).HasColumnName("unitoptr_id");

                entity.HasOne(d => d.Attr)
                    .WithMany(p => p.TblSubattrCdtabs)
                    .HasForeignKey(d => d.AttrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_subattr_cdtab_tbl_attr_cdtab");

                entity.HasOne(d => d.Attrunit)
                    .WithMany(p => p.TblSubattrCdtabs)
                    .HasForeignKey(d => d.AttrunitId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_subattr_cdtab_tbl_attrunit_cdtab");

                entity.HasOne(d => d.Unitoptr)
                    .WithMany(p => p.TblSubattrCdtabs)
                    .HasForeignKey(d => d.UnitoptrId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("fk_tbl_subattr_cdtab_tbl_unitoptr_cdtab");
            });

            modelBuilder.Entity<TblTrPjcostCdtab>(entity =>
            {
                entity.HasKey(e => e.TrPjcostCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_tr_pjcost_cdtab");

                entity.Property(e => e.TrPjcostCd).HasColumnName("tr_pjcost_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SeqNo)
                    .HasColumnType("decimal(3,2)")
                    .HasColumnName("seq_no");

                entity.Property(e => e.TrPjcostDet)
                    .IsRequired()
                    .HasMaxLength(40)
                    .HasColumnName("tr_pjcost_det");

                entity.Property(e => e.TrPjcostFlg)
                    .HasColumnType("tinyint")
                    .HasColumnName("tr_pjcost_flg");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblTrgEmpGrade>(entity =>
            {
                entity.HasKey(e => e.TgesCode)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_trg_emp_grade");

                entity.Property(e => e.TgesCode)
                    .HasMaxLength(5)
                    .HasColumnName("tges_code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.TegsOrder)
                    .HasColumnType("decimal(3,1)")
                    .HasColumnName("tegs_order");

                entity.Property(e => e.TgesDesc)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("tges_desc");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblTrgEmployee>(entity =>
            {
                entity.HasKey(e => e.TeyTicketNum)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_trg_employee");

                entity.HasIndex(e => e.EmpMobileNo, "uk_emp_mobile_no")
                    .IsUnique();

                entity.Property(e => e.TeyTicketNum)
                    .HasMaxLength(8)
                    .HasColumnName("tey_ticket_num");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpMobileNo)
                    .HasMaxLength(15)
                    .HasColumnName("emp_mobile_no");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.TeyAjoinDate)
                    .HasColumnType("date")
                    .HasColumnName("tey_ajoin_date");

                entity.Property(e => e.TeyAliasName)
                    .HasMaxLength(30)
                    .HasColumnName("tey_alias_name");

                entity.Property(e => e.TeyBirthDate)
                    .HasColumnType("date")
                    .HasColumnName("tey_birth_date");

                entity.Property(e => e.TeyBloodGroup)
                    .HasMaxLength(3)
                    .HasColumnName("tey_blood_group");

                entity.Property(e => e.TeyCasteCode)
                    .HasMaxLength(5)
                    .HasColumnName("tey_caste_code");

                entity.Property(e => e.TeyCategoryCode)
                    .HasMaxLength(5)
                    .HasColumnName("tey_category_code");

                entity.Property(e => e.TeyColourBlindness)
                    .HasMaxLength(1)
                    .HasColumnName("tey_colour_blindness");

                entity.Property(e => e.TeyConfirmDate)
                    .HasColumnType("date")
                    .HasColumnName("tey_confirm_date");

                entity.Property(e => e.TeyCurrentBasic)
                    .HasColumnType("decimal(14,2)")
                    .HasColumnName("tey_current_basic");

                entity.Property(e => e.TeyCurrentUnit)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("tey_current_unit");

                entity.Property(e => e.TeyDeleteStatus)
                    .HasMaxLength(2)
                    .HasColumnName("tey_delete_status");

                entity.Property(e => e.TeyDeptCode)
                    .HasMaxLength(5)
                    .HasColumnName("tey_dept_code");

                entity.Property(e => e.TeyEmpType)
                    .HasMaxLength(2)
                    .HasColumnName("tey_emp_type");

                entity.Property(e => e.TeyEmployeeStatus)
                    .HasMaxLength(1)
                    .HasColumnName("tey_employee_status");

                entity.Property(e => e.TeyEntryBasic)
                    .HasColumnType("decimal(14,2)")
                    .HasColumnName("tey_entry_basic");

                entity.Property(e => e.TeyEyeSight)
                    .HasMaxLength(1)
                    .HasColumnName("tey_eye_sight");

                entity.Property(e => e.TeyFatherHusbandName)
                    .HasMaxLength(60)
                    .HasColumnName("tey_father_husband_name");

                entity.Property(e => e.TeyFootwareSize).HasColumnName("tey_footware_size");

                entity.Property(e => e.TeyGradeCode)
                    .HasMaxLength(5)
                    .HasColumnName("tey_grade_code");

                entity.Property(e => e.TeyHomeCity)
                    .HasMaxLength(30)
                    .HasColumnName("tey_home_city");

                entity.Property(e => e.TeyHomeState)
                    .HasMaxLength(5)
                    .HasColumnName("tey_home_state");

                entity.Property(e => e.TeyJoinDate)
                    .HasColumnType("date")
                    .HasColumnName("tey_join_date");

                entity.Property(e => e.TeyLastdateIncrement)
                    .HasColumnType("date")
                    .HasColumnName("tey_lastdate_increment");

                entity.Property(e => e.TeyLastdatePromotion)
                    .HasColumnType("date")
                    .HasColumnName("tey_lastdate_promotion");

                entity.Property(e => e.TeyMaritalStatus)
                    .HasMaxLength(1)
                    .HasColumnName("tey_marital_status");

                entity.Property(e => e.TeyModeOfPay)
                    .HasMaxLength(1)
                    .HasColumnName("tey_mode_of_pay");

                entity.Property(e => e.TeyMotherTongue)
                    .HasMaxLength(5)
                    .HasColumnName("tey_mother_tongue");

                entity.Property(e => e.TeyName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("tey_name");

                entity.Property(e => e.TeyNationality)
                    .HasMaxLength(30)
                    .HasColumnName("tey_nationality");

                entity.Property(e => e.TeyNextIncrementDate)
                    .HasColumnType("date")
                    .HasColumnName("tey_next_increment_date");

                entity.Property(e => e.TeyPanNum)
                    .HasMaxLength(30)
                    .HasColumnName("tey_pan_num");

                entity.Property(e => e.TeyPayStatus)
                    .HasMaxLength(1)
                    .HasColumnName("tey_pay_status");

                entity.Property(e => e.TeyPermanentAddress1)
                    .HasMaxLength(60)
                    .HasColumnName("tey_permanent_address1");

                entity.Property(e => e.TeyPermanentAddress2)
                    .HasMaxLength(60)
                    .HasColumnName("tey_permanent_address2");

                entity.Property(e => e.TeyPermanentCity)
                    .HasMaxLength(30)
                    .HasColumnName("tey_permanent_city");

                entity.Property(e => e.TeyPermanentEmail)
                    .HasMaxLength(30)
                    .HasColumnName("tey_permanent_email");

                entity.Property(e => e.TeyPermanentPhone).HasColumnName("tey_permanent_phone");

                entity.Property(e => e.TeyPermanentState)
                    .HasMaxLength(30)
                    .HasColumnName("tey_permanent_state");

                entity.Property(e => e.TeyPermanentZip)
                    .HasMaxLength(30)
                    .HasColumnName("tey_permanent_zip");

                entity.Property(e => e.TeyPfNum)
                    .HasMaxLength(30)
                    .HasColumnName("tey_pf_num");

                entity.Property(e => e.TeyPresentAddress1)
                    .HasMaxLength(60)
                    .HasColumnName("tey_present_address1");

                entity.Property(e => e.TeyPresentAddress2)
                    .HasMaxLength(60)
                    .HasColumnName("tey_present_address2");

                entity.Property(e => e.TeyPresentCity)
                    .HasMaxLength(30)
                    .HasColumnName("tey_present_city");

                entity.Property(e => e.TeyPresentEmail)
                    .HasMaxLength(30)
                    .HasColumnName("tey_present_email");

                entity.Property(e => e.TeyPresentPhone).HasColumnName("tey_present_phone");

                entity.Property(e => e.TeyPresentState)
                    .HasMaxLength(30)
                    .HasColumnName("tey_present_state");

                entity.Property(e => e.TeyPresentZip)
                    .HasMaxLength(30)
                    .HasColumnName("tey_present_zip");

                entity.Property(e => e.TeyReligionCode)
                    .HasMaxLength(5)
                    .HasColumnName("tey_religion_code");

                entity.Property(e => e.TeySeparationDate)
                    .HasColumnType("date")
                    .HasColumnName("tey_separation_date");

                entity.Property(e => e.TeySeparationRef)
                    .HasColumnType("date")
                    .HasColumnName("tey_separation_ref");

                entity.Property(e => e.TeySeparationType)
                    .HasMaxLength(5)
                    .HasColumnName("tey_separation_type");

                entity.Property(e => e.TeySex)
                    .IsRequired()
                    .HasMaxLength(1)
                    .HasColumnName("tey_sex");

                entity.Property(e => e.TeySpouseName)
                    .HasMaxLength(60)
                    .HasColumnName("tey_spouse_name");

                entity.Property(e => e.TeyStaftypeCode)
                    .HasMaxLength(8)
                    .HasColumnName("tey_staftype_code");

                entity.Property(e => e.TeySuperUser)
                    .HasMaxLength(1)
                    .HasColumnName("tey_super_user");

                entity.Property(e => e.TeyUnitCode)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("tey_unit_code");

                entity.Property(e => e.TeyVpfPercent)
                    .HasColumnType("decimal(5,2)")
                    .HasColumnName("tey_vpf_percent");

                entity.Property(e => e.TeyWhetherHandicap)
                    .HasMaxLength(1)
                    .HasColumnName("tey_whether_handicap");

                entity.Property(e => e.TeyWorkArea)
                    .HasMaxLength(5)
                    .HasColumnName("tey_work_area");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblUnitoptrCdtab>(entity =>
            {
                entity.HasKey(e => e.UnitoptrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_unitoptr_cdtab");

                entity.Property(e => e.UnitoptrId).HasColumnName("unitoptr_id");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UnitoptrDesc)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("unitoptr_desc");
            });

            modelBuilder.Entity<TlqCdtab>(entity =>
            {
                entity.HasKey(e => e.TlqCd)
                    .HasName("PRIMARY");

                entity.ToTable("tlq_cdtab");

                entity.HasComment("Taluk Details");

                entity.HasIndex(e => e.DistCd, "fk_TLQ_CDTAB_DIST_CDTAB");

                entity.Property(e => e.TlqCd)
                    .HasColumnName("TLQ_CD")
                    .HasComment("Taluka Code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DistCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("DIST_CD")
                    .HasComment("District Code Ref: dist_cdtab (dist_cd)");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.TlqBhoomicode).HasColumnName("TLQ_BHOOMICODE");

                entity.Property(e => e.TlqIndzone)
                    .HasColumnType("tinyint")
                    .HasColumnName("TLQ_INDZONE");

                entity.Property(e => e.TlqLgdcode).HasColumnName("TLQ_LGDCODE");

                entity.Property(e => e.TlqNam)
                    .HasMaxLength(25)
                    .HasColumnName("TLQ_NAM")
                    .HasComment("Taluka Name");

                entity.Property(e => e.TlqNameKannada)
                    .HasMaxLength(100)
                    .HasColumnName("TLQ_NAME_KANNADA");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(d => d.DistCdNavigation)
                    .WithMany(p => p.TlqCdtabs)
                    .HasForeignKey(d => d.DistCd)
                    .HasConstraintName("fk_TLQ_CDTAB_DIST_CDTAB");
            });



            modelBuilder.Entity<UnitInfo1>(entity =>
            {
                entity.HasKey(e => e.UtCd)
                    .HasName("PRIMARY");

                entity.ToTable("unit_info1");

                entity.HasComment("Unit Details");

                entity.HasIndex(e => e.UtHob, "fk_UNIT_INFO1_HOB_CDTAB");

                entity.HasIndex(e => e.UtOffc, "fk_UNIT_INFO1_OFFC_CDTAB");

                entity.HasIndex(e => e.UtVil, "fk_UNIT_INFO1_VIL_CDTAB");

                entity.HasIndex(e => e.UtInd, "fk_UNIT_INFO1_tbl_ind_cdtab");

                entity.HasIndex(e => e.UtSize, "fk_UNIT_INFO1_tbl_size_cdtab");

                entity.Property(e => e.UtCd)
                    .HasColumnName("UT_CD")
                    .HasComment("Unit Code");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UnitId)
                    .HasColumnType("tinyint")
                    .HasColumnName("UNIT_ID");

                entity.Property(e => e.UtBankAcno)
                    .HasMaxLength(6)
                    .HasColumnName("UT_BANK_ACNO")
                    .HasComment("Bank Account No.");

                entity.Property(e => e.UtBankAdr1)
                    .HasMaxLength(40)
                    .HasColumnName("UT_BANK_ADR1")
                    .HasComment("Bank Address 1");

                entity.Property(e => e.UtBankAdr2)
                    .HasMaxLength(40)
                    .HasColumnName("UT_BANK_ADR2")
                    .HasComment("Bank Address 2");

                entity.Property(e => e.UtBankAdr3)
                    .HasMaxLength(40)
                    .HasColumnName("UT_BANK_ADR3")
                    .HasComment("Bank Address 3");

                entity.Property(e => e.UtBankAdr4)
                    .HasMaxLength(40)
                    .HasColumnName("UT_BANK_ADR4")
                    .HasComment("Bank Address 4");

                entity.Property(e => e.UtBankTel1)
                    .HasColumnName("UT_BANK_TEL1")
                    .HasComment("Bank Telephone 1");

                entity.Property(e => e.UtBankTel2)
                    .HasColumnName("UT_BANK_TEL2")
                    .HasComment("Bank Telephone 2");

                entity.Property(e => e.UtBanker)
                    .HasMaxLength(120)
                    .HasColumnName("UT_BANKER")
                    .HasComment("Name of Banker");

                entity.Property(e => e.UtCadrs)
                    .HasMaxLength(1)
                    .HasColumnName("UT_CADRS")
                    .HasComment("Correspondence Address (R / O / F) unit_id	Branch Office Code");

                entity.Property(e => e.UtChfProm)
                    .HasColumnName("UT_CHF_PROM")
                    .HasComment("Chief Promoter Code (Ref: xxxx)");

                entity.Property(e => e.UtCnst)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_CNST")
                    .HasComment("Unit Constitution Ref: cnst_cdtab (cnst_cd)");

                entity.Property(e => e.UtCorpCd)
                    .HasColumnName("UT_CORP_CD")
                    .HasComment("Account no. from where it is transferred");

                entity.Property(e => e.UtDist)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_DIST")
                    .HasComment("District Code Ref: dist_cdtab (dist_cd)");

                entity.Property(e => e.UtEmail)
                    .HasMaxLength(40)
                    .HasColumnName("UT_EMAIL")
                    .HasComment("Unit E-Mail Address");

                entity.Property(e => e.UtFadr1)
                    .HasMaxLength(50)
                    .HasColumnName("UT_FADR1")
                    .HasComment("Factory Address 1");

                entity.Property(e => e.UtFadr2)
                    .HasMaxLength(50)
                    .HasColumnName("UT_FADR2")
                    .HasComment("Factory Address 2");

                entity.Property(e => e.UtFadr3)
                    .HasMaxLength(50)
                    .HasColumnName("UT_FADR3")
                    .HasComment("Factory Address 3");

                entity.Property(e => e.UtFcollab)
                    .HasMaxLength(50)
                    .HasColumnName("UT_FCOLLAB")
                    .HasComment("Foreign Collaboration");

                entity.Property(e => e.UtFstdCd)
                    .HasMaxLength(20)
                    .HasColumnName("UT_FSTD_CD")
                    .HasComment("Factory STD Code");

                entity.Property(e => e.UtFtel)
                    .HasColumnName("UT_FTEL")
                    .HasComment("Factory Telephone");

                entity.Property(e => e.UtGstNo)
                    .HasMaxLength(15)
                    .HasColumnName("UT_GST_NO")
                    .HasComment("GST Number");

                entity.Property(e => e.UtHob)
                    .HasColumnName("UT_HOB")
                    .HasComment("Hobli Code Ref: hob_cdtab (hob_cd)");

                entity.Property(e => e.UtIncDate)
                    .HasColumnName("UT_INC_DATE")
                    .HasComment("Firm Incorporation Date");

                entity.Property(e => e.UtInd).HasColumnName("UT_IND");

                entity.Property(e => e.UtKzone)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_KZONE")
                    .HasComment("Zone Code Ref: kzn_cdtab (kzn_cd)");

                entity.Property(e => e.UtName)
                    .HasMaxLength(40)
                    .HasColumnName("UT_NAME")
                    .HasComment("Unit Name");

                entity.Property(e => e.UtNriInvst)
                    .HasColumnName("UT_NRI_INVST")
                    .HasComment("NRI Investment Code");

                entity.Property(e => e.UtOadr1)
                    .HasMaxLength(50)
                    .HasColumnName("UT_OADR1")
                    .HasComment("Office Address 1");

                entity.Property(e => e.UtOadr2)
                    .HasMaxLength(50)
                    .HasColumnName("UT_OADR2")
                    .HasComment("Office Address 2");

                entity.Property(e => e.UtOadr3)
                    .HasMaxLength(50)
                    .HasColumnName("UT_OADR3")
                    .HasComment("Office Address 3");

                entity.Property(e => e.UtOfax)
                    .HasMaxLength(15)
                    .HasColumnName("UT_OFAX")
                    .HasComment("Office Fax");

                entity.Property(e => e.UtOffc)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_OFFC")
                    .HasComment("Branch Code Ref: offc_cdtab (offc_cd)");

                entity.Property(e => e.UtOgrms)
                    .HasMaxLength(10)
                    .HasColumnName("UT_OGRMS")
                    .HasComment("Office Telegrams");

                entity.Property(e => e.UtOpin)
                    .HasColumnName("UT_OPIN")
                    .HasComment("Office Pincode");

                entity.Property(e => e.UtOstdCd1)
                    .HasMaxLength(6)
                    .HasColumnName("UT_OSTD_CD1")
                    .HasComment("Office STD Code 1");

                entity.Property(e => e.UtOstdCd2)
                    .HasMaxLength(6)
                    .HasColumnName("UT_OSTD_CD2")
                    .HasComment("Office STD Code 2");

                entity.Property(e => e.UtOtel1)
                    .HasColumnName("UT_OTEL1")
                    .HasComment("Office Telephone 1");

                entity.Property(e => e.UtOtel2)
                    .HasColumnName("UT_OTEL2")
                    .HasComment("Office Telephone 2");

                entity.Property(e => e.UtOtlx)
                    .HasMaxLength(200)
                    .HasColumnName("UT_OTLX")
                    .HasComment("Office Telex");

                entity.Property(e => e.UtPan)
                    .HasMaxLength(10)
                    .HasColumnName("UT_PAN")
                    .HasComment("PAN of Unit");

                entity.Property(e => e.UtPin)
                    .HasColumnName("UT_PIN")
                    .HasComment("Pincode");

                entity.Property(e => e.UtProd1)
                    .HasColumnName("UT_PROD1")
                    .HasComment("Product Code");

                entity.Property(e => e.UtProd2)
                    .HasColumnName("UT_PROD2")
                    .HasComment("Product Code");

                entity.Property(e => e.UtProd3)
                    .HasColumnName("UT_PROD3")
                    .HasComment("Product Code");

                entity.Property(e => e.UtProd4)
                    .HasColumnName("UT_PROD4")
                    .HasComment("Product Code");

                entity.Property(e => e.UtPromOffc)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_PROM_OFFC")
                    .HasComment("Branch Office Code (for transfer of Unit)");

                entity.Property(e => e.UtRadr1)
                    .HasMaxLength(50)
                    .HasColumnName("UT_RADR1")
                    .HasComment("Registered Address 1");

                entity.Property(e => e.UtRadr2)
                    .HasMaxLength(50)
                    .HasColumnName("UT_RADR2")
                    .HasComment("Registered Address 2");

                entity.Property(e => e.UtRadr3)
                    .HasMaxLength(50)
                    .HasColumnName("UT_RADR3")
                    .HasComment("Registered Address 3");

                entity.Property(e => e.UtRebType)
                    .HasMaxLength(2)
                    .HasColumnName("UT_REB_TYPE")
                    .HasComment("Rebate Type (Rebate Old / Rebate New ) RO/RN");

                entity.Property(e => e.UtRetelx)
                    .HasMaxLength(10)
                    .HasColumnName("UT_RETELX")
                    .HasComment("Registered Telex");

                entity.Property(e => e.UtRfax)
                    .HasMaxLength(15)
                    .HasColumnName("UT_RFAX")
                    .HasComment("Registered Fax");

                entity.Property(e => e.UtRgrms)
                    .HasMaxLength(10)
                    .HasColumnName("UT_RGRMS")
                    .HasComment("Registered Telegrams");

                entity.Property(e => e.UtRstdCd)
                    .HasMaxLength(6)
                    .HasColumnName("UT_RSTD_CD")
                    .HasComment("Corporate Code");

                entity.Property(e => e.UtRtel1)
                    .HasColumnName("UT_RTEL1")
                    .HasComment("Registered Telephone 1");

                entity.Property(e => e.UtRtel2)
                    .HasColumnName("UT_RTEL2")
                    .HasComment("Registered Telephone 2");

                entity.Property(e => e.UtSize).HasColumnName("UT_SIZE");

                entity.Property(e => e.UtSzone)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_SZONE")
                    .HasComment("State Zone Code");

                entity.Property(e => e.UtTlq)
                    .HasColumnType("tinyint")
                    .HasColumnName("UT_TLQ")
                    .HasComment("Taluka Code Ref: tlq_cdtab (tlq_cd)");

                entity.Property(e => e.UtVil)
                    .HasColumnName("UT_VIL")
                    .HasComment("Village Code Ref: vil_cdtab (vil_cd)");

                entity.Property(e => e.UtWeb)
                    .HasMaxLength(40)
                    .HasColumnName("UT_WEB")
                    .HasComment("Unit Website Address");

                //entity.HasOne(d => d.UtHobNavigation)
                //    .WithMany(p => p.UnitInfo1s)
                //    .HasForeignKey(d => d.UtHob)
                //    .HasConstraintName("fk_UNIT_INFO1_HOB_CDTAB");

                entity.HasOne(d => d.UtIndNavigation)
                    .WithMany(p => p.UnitInfo1s)
                    .HasForeignKey(d => d.UtInd)
                    .HasConstraintName("fk_UNIT_INFO1_tbl_ind_cdtab");

                entity.HasOne(d => d.UtOffcNavigation)
                    .WithMany(p => p.UnitInfo1s)
                    .HasForeignKey(d => d.UtOffc)
                    .HasConstraintName("fk_UNIT_INFO1_OFFC_CDTAB");

                entity.HasOne(d => d.UtSizeNavigation)
                    .WithMany(p => p.UnitInfo1s)
                    .HasForeignKey(d => d.UtSize)
                    .HasConstraintName("fk_UNIT_INFO1_tbl_size_cdtab");

                entity.HasOne(d => d.UtVilNavigation)
                    .WithMany(p => p.UnitInfo1s)
                    .HasForeignKey(d => d.UtVil)
                    .HasConstraintName("fk_UNIT_INFO1_VIL_CDTAB");
            });

            modelBuilder.Entity<UnitMaptab>(entity =>
            {
                entity.HasKey(e => e.UnitMapId)
                    .HasName("PRIMARY");

                entity.ToTable("unit_maptab");

                entity.HasComment("Unit Mapping Table");

                entity.Property(e => e.UnitMapId)
                    .HasColumnType("tinyint")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UNIT_MAP_ID");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.OffcCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("OFFC_CD")
                    .HasComment("Branch Office Code");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.UtCd)
                    .HasColumnName("UT_CD")
                    .HasComment("New Unit ID");

                entity.Property(e => e.UtOld)
                    .HasColumnName("UT_OLD")
                    .HasComment("Old Unit ID");
            });

            modelBuilder.Entity<VilCdtab>(entity =>
            {
                entity.HasKey(e => e.VilCd)
                    .HasName("PRIMARY");

                entity.ToTable("vil_cdtab");

                entity.HasComment("Village Details");

                entity.HasIndex(e => e.ConstmlaCd, "fk_VIL_CDTAB_CONSTMLA_CDTAB");

                entity.HasIndex(e => e.ConstmpCd, "fk_VIL_CDTAB_CONSTMP_CDTAB");

                entity.Property(e => e.VilCd).HasColumnName("VIL_CD");

                entity.Property(e => e.ConstmlaCd)
                    .HasColumnName("CONSTMLA_CD")
                    .HasComment("MLA Constituency Code Ref: constmla_cdtab (constmla_cd)");

                entity.Property(e => e.ConstmpCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("CONSTMP_CD")
                    .HasComment("MP Constiutency Code Ref: constmp_cdtab (constmp_cd)");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.HobCd)
                    .HasColumnName("HOB_CD")
                    .HasComment("Hobli Code Ref: hob_cdtab (hob_cd)");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.VilBhoomicode)
                    .HasColumnType("decimal(20,0)")
                    .HasColumnName("VIL_BHOOMICODE")
                    .HasComment("LGD code from Bhoomi Project");

                entity.Property(e => e.VilLgdcode)
                    .HasColumnType("decimal(20,0)")
                    .HasColumnName("VIL_LGDCODE")
                    .HasComment("LGD code from KSRSAC");

                entity.Property(e => e.VilNam)
                    .HasMaxLength(25)
                    .HasColumnName("VIL_NAM")
                    .HasComment("Village Name");

                entity.Property(e => e.VilNameKannada)
                    .HasMaxLength(50)
                    .HasColumnName("VIL_NAME_KANNADA")
                    .HasComment("Village Name in Kannada");

                entity.HasOne(d => d.ConstmlaCdNavigation)
                    .WithMany(p => p.VilCdtabs)
                    .HasForeignKey(d => d.ConstmlaCd)
                    .HasConstraintName("fk_VIL_CDTAB_CONSTMLA_CDTAB");

                entity.HasOne(d => d.ConstmpCdNavigation)
                    .WithMany(p => p.VilCdtabs)
                    .HasForeignKey(d => d.ConstmpCd)
                    .HasConstraintName("fk_VIL_CDTAB_CONSTMP_CDTAB");
            });
            modelBuilder.Entity<EnqAckDet>(entity =>
            {
                entity.HasKey(e => e.EnqRowId).HasName("PRIMARY");

                entity.Property(e => e.EnqRowId)
                    .HasColumnType("int")
                    .ValueGeneratedOnAdd()
                    .HasColumnName("enq_rowid");
                entity.ToTable("enq_ack_det");

                entity.Property(e => e.EnqRefNo).HasColumnName("enq_ref_no");


                entity.Property(e => e.EmpId).HasColumnName("emp_id");
                entity.Property(e => e.EnqAckDate).HasColumnName("enq_ack_date");

            });

            modelBuilder.Entity<TblAppTeamDet>(entity =>
            {
                entity.HasKey(e => e.AppTeamRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_team_det");

                entity.HasIndex(e => e.UtCd, "FK_tbl_app_team_det_tbl_app_unit_details");

                entity.HasIndex(e => e.EmpId, "FK_tbl_app_team_det_tbl_trg_employee");

                entity.Property(e => e.AppTeamRowid).HasColumnName("app_team_rowid");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EmpId)
                    .IsRequired()
                    .HasMaxLength(8)
                    .HasColumnName("emp_id");

                entity.Property(e => e.EnqRefNo).HasColumnName("enq_ref_no");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.HasOne(d => d.UtCdNavigation)
                    .WithMany(p => p.TblAppTeamDets)
                    .HasForeignKey(d => d.UtCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_app_team_det_tbl_app_unit_details");
            });

            modelBuilder.Entity<TblAppUnitAddress>(entity =>
            {
                entity.HasKey(e => e.UtAddressRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_unit_address");

                entity.HasIndex(e => e.AddtypeCd, "FK_tbl_app_unit_address_tbl_address_cdtab");

                entity.HasIndex(e => e.UtCd, "FK_tbl_app_unit_address_tbl_app_unit_details");

                entity.Property(e => e.UtAddressRowid).HasColumnName("ut_address_rowid");

                entity.Property(e => e.AddtypeCd).HasColumnName("addtype_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqRefNo).HasColumnName("enq_ref_no");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UtAddress)
                    .HasMaxLength(200)
                    .HasColumnName("ut_address");

                entity.Property(e => e.UtAltEmail)
                    .HasMaxLength(50)
                    .HasColumnName("ut_alt_email");

                entity.Property(e => e.UtAltMobile).HasColumnName("ut_alt_mobile");

                entity.Property(e => e.UtArea)
                    .HasMaxLength(100)
                    .HasColumnName("ut_area");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.Property(e => e.UtCity)
                    .HasMaxLength(100)
                    .HasColumnName("ut_city");

                entity.Property(e => e.UtEmail)
                    .HasMaxLength(50)
                    .HasColumnName("ut_email");

                entity.Property(e => e.UtMobile).HasColumnName("ut_mobile");

                entity.Property(e => e.UtPincode).HasColumnName("ut_pincode");

                entity.Property(e => e.UtTelephone).HasColumnName("ut_telephone");

                entity.HasOne(d => d.UtCdNavigation)
                    .WithMany(p => p.TblAppUnitAddresses)
                    .HasForeignKey(d => d.UtCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_app_unit_address_tbl_app_unit_details");
            });

            modelBuilder.Entity<TblAppUnitBank>(entity =>
            {
                entity.HasKey(e => e.UtBankRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_unit_bank");

                entity.HasIndex(e => e.UtCd, "FK_tbl_app_unit_bank_tbl_app_unit_details");

                entity.Property(e => e.UtBankRowid).HasColumnName("ut_bank_rowid");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EnqRefNo).HasColumnName("enq_ref_no");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UtBank)
                    .HasMaxLength(100)
                    .HasColumnName("ut_bank");

                entity.Property(e => e.UtBankAddress)
                    .HasMaxLength(200)
                    .HasColumnName("ut_bank_address");

                entity.Property(e => e.UtBankArea)
                    .HasMaxLength(100)
                    .HasColumnName("ut_bank_area");

                entity.Property(e => e.UtBankBranch)
                    .HasMaxLength(100)
                    .HasColumnName("ut_bank_branch");

                entity.Property(e => e.UtBankCity)
                    .HasMaxLength(100)
                    .HasColumnName("ut_bank_city");

                entity.Property(e => e.UtBankPhone).HasColumnName("ut_bank_phone");

                entity.Property(e => e.UtBankPrimary).HasColumnName("ut_bank_primary");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.Property(e => e.UtIfsc)
                    .HasMaxLength(10)
                    .HasColumnName("ut_ifsc");

                entity.HasOne(d => d.UtCdNavigation)
                    .WithMany(p => p.TblAppUnitBanks)
                    .HasForeignKey(d => d.UtCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_app_unit_bank_tbl_app_unit_details");
            });

            modelBuilder.Entity<TblAppUnitDetail>(entity =>
            {
                entity.HasKey(e => e.UtRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_unit_details");

                entity.HasIndex(e => e.CnstCd, "FK_tbl_app_unit_details_kzn_cdtab");

                entity.HasIndex(e => e.IndCd, "FK_tbl_app_unit_details_tbl_ind_cdtab");

                entity.HasIndex(e => e.SizeCd, "FK_tbl_app_unit_details_tbl_size_cdtab");

                entity.HasIndex(e => e.UtCd, "fk_tbl_app_unit_details_tbl_unit_mast");

                entity.Property(e => e.UtRowid).HasColumnName("ut_rowid");

                entity.Property(e => e.CnstCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("cnst_cd");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.EgNo).HasColumnName("eg_no");

                entity.Property(e => e.IncorporationDt)
                    .HasColumnType("date")
                    .HasColumnName("incorporation_dt");

                entity.Property(e => e.IndCd).HasColumnName("ind_cd");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.KznCd)
                    .HasColumnType("tinyint")
                    .HasColumnName("kzn_cd");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.SizeCd).HasColumnName("size_cd");

                entity.Property(e => e.UnitGstin)
                    .HasMaxLength(15)
                    .HasColumnName("unit_gstin");

                entity.Property(e => e.UnitPan)
                    .HasMaxLength(10)
                    .HasColumnName("unit_pan");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.Property(e => e.UtName).HasColumnName("ut_name");

                entity.Property(e => e.UtZone)
                    .HasMaxLength(20)
                    .HasColumnName("ut_zone");

                entity.HasOne(e => e.ProdIndNavigation)
                .WithOne(b => b.UtCdNavigation)
                .HasForeignKey<TblAppUnitDetail>(b => b.IndCd);
            });

            modelBuilder.Entity<TblAppUnitLoanDet>(entity =>
            {
                entity.HasKey(e => e.UtLoanRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_unit_loan_det");

                entity.HasIndex(e => e.PurpCd, "FK_tbl_app_unit_loan_det_tbl_purp_cdtab");

                entity.HasIndex(e => e.UtCd, "FK_tbl_app_unit_products_tbl_app_unit_loan_det");

                entity.Property(e => e.UtLoanRowid).HasColumnName("ut_loan_rowid");

                entity.Property(e => e.ActivityType).HasColumnName("activity_type");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.DateInfoComplete)
                    .HasColumnType("date")
                    .HasColumnName("date_info_complete");

                entity.Property(e => e.DateInfoReceive)
                    .HasColumnType("date")
                    .HasColumnName("date_info_receive");

                entity.Property(e => e.EnqRefNo).HasColumnName("enq_ref_no");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.LoanType).HasColumnName("loan_type");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.PurpCd).HasColumnName("purp_cd");

                entity.Property(e => e.SchemeCd).HasColumnName("scheme_cd");

                entity.Property(e => e.SubsidyType).HasColumnName("subsidy_type");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.Property(e => e.UtLoanAmt)
                    .HasColumnType("decimal(10,2)")
                    .HasColumnName("ut_loan_amt");

                entity.HasOne(d => d.UtCdNavigation)
                    .WithMany(p => p.TblAppUnitLoanDets)
                    .HasForeignKey(d => d.UtCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_app_unit_products_tbl_app_unit_loan_det");
            });

            modelBuilder.Entity<TblAppUnitProduct>(entity =>
            {
                entity.HasKey(e => e.UtProductRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_unit_products");

                entity.HasIndex(e => e.UtCd, "FK_tbl_app_unit_products_tbl_app_unit_details");

                entity.Property(e => e.UtProductRowid).HasColumnName("ut_product_rowid");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.ProdCd).HasColumnName("prod_cd");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.HasOne(d => d.UtCdNavigation)
                    .WithMany(p => p.TblAppUnitProducts)
                    .HasForeignKey(d => d.UtCd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tbl_app_unit_products_tbl_app_unit_details");
            });

            /// <summary>
            ///  Author: Gagana K; Module:Hypothecation; Date:21/07/2022
            /// </summary>
            modelBuilder.Entity<TblIdmHypothDet>(entity =>
            {

                entity.HasKey(e => e.IdmHypothDetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_hypoth_det");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_hypoth_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_hypoth_det_offc_cdtab");

                entity.HasIndex(e => e.AssetCd, "fk_tbl_idm_hypoth_det_tbl_asset_refno_det");
                entity.HasIndex(e => e.ApprovedEmpId, "fk_tbl_idm_hypoth_det_tbl_idm_dsb_spl_cleg");

                entity.Property(e => e.IdmHypothDetId)
                    .HasColumnName("idm_hypoth_detid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.AssetCd)
                    .HasColumnName("asset_cd");

                entity.Property(e => e.HypothNo)
                    .HasMaxLength(20)
                    .HasColumnName("hypoth_no");

                entity.Property(e => e.HypothDesc)
                 .HasMaxLength(200)
                 .HasColumnName("hypoth_desc");

                entity.Property(e => e.AssetValue)
                    .HasColumnName("asset_value");

                entity.Property(e => e.ExecutionDate)
                    .HasColumnName("execution_date");

                entity.Property(e => e.HypothUpload)
                .HasMaxLength(300)
                .HasColumnName("hypoth_upload");

                entity.Property(e => e.ApprovedEmpId)
               .HasMaxLength(8)
               .HasColumnName("approved_emp_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                //entity.Property(e => e.UtCd)
                //   .HasColumnName("ut_cd");

                //entity.Property(e => e.AssetItemNo)
                //  .HasColumnName("asset_item_no");

                //entity.Property(e => e.AssetQty)
                //  .HasColumnName("asset_qty");

                entity.Property(e => e.AssetName)
                  .HasColumnName("asset_name");

                entity.Property(e => e.AssetDet)
                  .HasColumnName("asset_details");

                //entity.Property(e => e.UtSlno)
                //  .HasColumnName(" ut_slno ");




                //entity.HasOne(e => e.TblAssetRefnoDet)
                //       .WithOne(b => b.TblIdmHypothDet)
                //       .HasForeignKey<TblIdmHypothDet>(b => b.AssetCd);


                //entity.HasOne(e => e.TblAssetRefnoDet)
                //.WithMany()
                //.HasForeignKey(c => c.AssetCd)
                //.HasConstraintName("fk_tbl_idm_hypoth_det_tbl_asset_refno_det");

                entity.HasOne(e => e.TblAssetRefnoDet)
                .WithMany(h => h.TblIdmHypothDet)
                .HasForeignKey(c => c.AssetCd);

            });

            modelBuilder.Entity<TblIdmHypothMap>(entity =>
            {
                entity.HasKey(e => e.HypothMapId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_hypoth_map");


                entity.Property(e => e.HypothMapId)
                 .HasColumnName("hypoth_map_id");

                entity.Property(e => e.HypothCode)
                  .HasMaxLength(20)
                  .HasColumnName("hypoth_code");

                entity.Property(e => e.HypothNo)
                 .HasMaxLength(20)
                 .HasColumnName("hypoth_no");

                entity.Property(e => e.HypothDeedNo)
                 .HasMaxLength(20)
                    .HasColumnName("hypoth_deed_no");

                entity.Property(e => e.HypothValue)
                 .HasMaxLength(20)
                    .HasColumnName("hypoth_value");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");
            });


            /// <summary>
            ///  Author: Gagana K; Module: Hypothecation; Date:21/07/2022
            /// </summary>
            modelBuilder.Entity<TblAssetRefnoDet>(entity =>
            {

                entity.HasKey(e => e.AssetRefnoMastId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_asset_refno_det");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_asset_refno_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_asset_refno_det_offc_cdtab");

                entity.HasIndex(e => e.AssetcatCd, "fk_tbl_asset_refno_det_tbl_assetcat_cdtab");

                entity.HasIndex(e => e.AssetypeCd, "fk_tbl_asset_refno_det_tbl_assettype_cdtab");

                entity.Property(e => e.AssetRefnoMastId)
                    .HasColumnName("asset_refno_mast_id");

                entity.Property(e => e.AssetCd)
                    .HasColumnName("asset_cd");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.AssetcatCd)
                    .HasColumnName("assetcat_cd");

                entity.Property(e => e.AssetypeCd)
                    .HasColumnName("assettype_cd");

                entity.Property(e => e.AssetDetails)
                    .HasMaxLength(200)
                    .HasColumnName("asset_details");

                entity.Property(e => e.AssetValue)
                    .HasColumnName("asset_value");

                entity.Property(e => e.AssetOthDetails)
                                    .HasMaxLength(500)
                    .HasColumnName("asset_othdetails");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.WhHyp)
                   .HasColumnName("wh_hyp");

                entity.Property(e => e.WhCersai)
                    .HasColumnName("wh_cersai");

                entity.HasOne(e => e.TblAssettypeCdtab)
                       .WithOne(b => b.TblAssetRefnoDet)
                       .HasForeignKey<TblAssetRefnoDet>(b => b.AssetypeCd);

                entity.HasOne(e => e.TblAssetcatCdtab)
                      .WithOne(b => b.TblAssetRefnoDet)
                      .HasForeignKey<TblAssetRefnoDet>(b => b.AssetcatCd);

            });

            /// <summary>
            ///  Author: Gagana K; Module: SecurityCharge; Date:21/07/2022
            /// </summary>
            modelBuilder.Entity<TblIdmDsbCharge>(entity =>
            {
                entity.HasKey(e => e.IdmDsbChargeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dsb_charge");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_asset_refno_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_asset_refno_det_offc_cdtab");

                entity.HasIndex(e => e.BankIfscCd, "fk_tbl_idm_dsb_charge_tbl_ifsc_master_id");

                entity.HasIndex(e => e.ChargeTypeCd, "fk_tbl_idm_dsb_charge_tbl_charge_type");

                entity.HasIndex(e => e.SecurityCd, "fk_tbl_idm_dsb_charge_tbl_security_refno_mast");
                entity.HasIndex(e => e.ApprovedEmpId, "fk_tbl_idm_dsb_charge_tbl_trg_employee");

                entity.Property(e => e.IdmDsbChargeId)
                    .HasColumnName("idm_dsb_charge_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.SecurityCd)
                    .HasColumnName("security_cd");

                entity.Property(e => e.ChargeTypeCd)
                    .HasColumnName("charge_type_cd");

                entity.Property(e => e.SecurityValue)
                   .HasColumnName("security_value");

                entity.Property(e => e.NocIssueBy)
                                    .HasMaxLength(100)
                    .HasColumnName("noc_issue_by");

                entity.Property(e => e.NocIssueTo)
                                    .HasMaxLength(100)
                    .HasColumnName("noc_isssue_to");

                entity.Property(e => e.NocDate)
                   .HasColumnName("noc_date");

                entity.Property(e => e.AuthLetterBy)
                                   .HasMaxLength(100)
                   .HasColumnName("auth_letter_by");

                entity.Property(e => e.AuthLetterDate)
                   .HasColumnName("auth_letter_date");

                entity.Property(e => e.BoardResolutionDate)
                   .HasColumnName("board_resolution_date");

                entity.Property(e => e.MoeInsuredDate)
                  .HasColumnName("moe_insured_date");

                entity.Property(e => e.RequestLtrNo)
                                  .HasMaxLength(50)
                  .HasColumnName("request_ltr_no");

                entity.Property(e => e.RequestLtrDate)
                 .HasColumnName("request_ltr_date");

                entity.Property(e => e.BankIfscCd)
                         .HasMaxLength(11)
                  .HasColumnName("bank_ifsc_cd");

                entity.Property(e => e.BankRequestLtrNo)
                                  .HasMaxLength(50)
                  .HasColumnName("bank_request_ltr_no");

                entity.Property(e => e.BankRequestLtrDate)
                .HasColumnName("bank_request_ltr_date");

                entity.Property(e => e.ChargePurpose)
                                  .HasMaxLength(500)
                  .HasColumnName("charge_purpose");

                entity.Property(e => e.ChargeDetails)
                                  .HasMaxLength(500)
                  .HasColumnName("charge_details");

                entity.Property(e => e.ChargeConditions)
                                  .HasMaxLength(500)
                  .HasColumnName("charge_conditions");

                entity.Property(e => e.UploadDocument)
                                .HasMaxLength(300)
                .HasColumnName("upload_document");

                entity.Property(e => e.ApprovedEmpId)
                                .HasMaxLength(8)
                .HasColumnName("approved_emp_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.BankIfscId)
                    .HasColumnName("bank_ifsc_id_cd");

                entity.Property(e => e.SecurityDets)
                   .HasColumnName("security_dets");

                entity.HasOne(e => e.TblSecurityRefnoMast)
                       .WithOne(b => b.TblIdmDsbCharge)
                       .HasForeignKey<TblIdmDsbCharge>(b => b.SecurityCd);

                entity.HasOne(e => e.TblChargeType)
                       .WithOne(b => b.TblIdmDsbCharge)
                       .HasForeignKey<TblIdmDsbCharge>(b => b.ChargeTypeCd);

                entity.HasOne(e => e.TbIIfscMaster)
                    .WithOne(b => b.TblIdmDsbCharge)
                    .HasForeignKey<TblIdmDsbCharge>(b => b.BankIfscId);

            });

            modelBuilder.Entity<TblIdmDspInsp>(entity =>
            {
                entity.HasKey(e => e.DinRowID)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dsp_insp");

                //entity.HasIndex(e => e.LoanAcc, "idm_unit_details_ibfk_1");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_dsp_insp_offc_cdtab");

                entity.Property(e => e.DinRowID)
                    .HasColumnName("din_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DinNo)
                    .HasColumnName("din_no");

                entity.Property(e => e.DinDt)
                    .HasColumnName("din_dt");

                entity.Property(e => e.DinTeam)
                   .HasColumnName("din_team");

                entity.Property(e => e.DinDept)
                    .HasColumnName("din_dept");

                entity.Property(e => e.DinRdt)
                    .HasColumnName("din_rdt");

                entity.Property(e => e.DinSeccd)
                   .HasColumnName("din_seccd");

                entity.Property(e => e.DinSecAmt)
                   .HasColumnName("din_secamt");

                entity.Property(e => e.DinSecrt)
                   .HasColumnName("din_secrt");

                entity.Property(e => e.Dinland)
                   .HasColumnName("din_land");

                entity.Property(e => e.DinLandArea)
                  .HasColumnName("din_land_area");

                entity.Property(e => e.DinLandDev)
                  .HasColumnName("din_land_dev");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            modelBuilder.Entity<TblIdmUnitDetails>(entity =>
            {
                entity.HasKey(e => e.IdmUtId)
                    .HasName("PRIMARY");

                entity.ToTable("idm_unit_details");

                entity.HasIndex(e => e.OffcCd, "idm_unit_details_ibfk_2");
                entity.HasIndex(e => e.CnstCd, "idm_unit_details_ibfk_3");
                entity.HasIndex(e => e.KznCd, "idm_unit_details_ibfk_4");
                entity.HasIndex(e => e.SizeCd, "idm_unit_details_ibfk_5");
                entity.HasIndex(e => e.IndCd, "idm_unit_details_ibfk_6");
                entity.HasIndex(e => e.LoanAcc, "idm_unit_details_idfk_7");
                entity.HasIndex(e => e.UnitDetailsName, "idm_unit_details_idfk_11");

                entity.Property(e => e.IdmUtId)
                   .HasColumnName("idm_ut_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                 .HasColumnName("ut_cd");

                entity.Property(e => e.UnitDetailsName)
                .HasColumnName("unitdetails_name");

                entity.Property(e => e.CnstCd)
               .HasColumnName("cnst_cd");

                entity.Property(e => e.IncorporationDt)
             .HasColumnName("incorporation_dt");

                entity.Property(e => e.KznCd)
             .HasColumnName("kzn_cd");

                entity.Property(e => e.UtZone)
             .HasColumnName("ut_zone");

                entity.Property(e => e.SizeCd)
             .HasColumnName("size_cd");

                entity.Property(e => e.UnitPan)
             .HasColumnName("unit_pan");

                entity.Property(e => e.UnitGstin)
             .HasColumnName("unit_gstin");

                entity.Property(e => e.IndCd)
             .HasColumnName("ind_cd");

                //entity.Property(e => e.IsActive)
                //    .HasDefaultValue(true)
                //    .HasColumnName("is_active");

                //entity.Property(e => e.IsDeleted)
                //    .HasDefaultValue(false)
                //    .HasColumnName("is_deleted");

                //entity.Property(e => e.CreateBy)
                //    .HasMaxLength(50)
                //    .HasColumnName("created_by");

                //entity.Property(e => e.ModifiedBy)
                //    .HasMaxLength(50)
                //    .HasColumnName("modified_by");

                //entity.Property(e => e.CreatedDate)
                //    .HasColumnName("created_date");

                //entity.Property(e => e.ModifiedDate)
                //    .HasColumnName("modified_date");

                //entity.Property(e => e.UniqueID)
                //                .HasMaxLength(200)
                //    .HasColumnName("unique_id");

                entity.HasOne(e => e.TblUnitMast)
                    .WithOne(b => b.TblIdmUnitDetails)
                    .HasForeignKey<TblIdmUnitDetails>(b => b.UnitDetailsName);

            });


            /// <summary>
            ///  Author: Gagana K; Module: CERSAI; Date:27/07/2022
            /// </summary>
            modelBuilder.Entity<TblIdmCersaiRegistration>(entity =>
            {
                entity.HasKey(e => e.IdmDsbChargeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_cersai_registration");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_cersai_registration_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");

                entity.HasIndex(e => e.AssetCd, "fk_tbl_idm_cersai_registration_tbl_asset_refno_det");

                entity.HasIndex(e => e.SecurityCd, "fk_tbl_idm_cersai_registration_tbl_security_refno_mast");
                entity.HasIndex(e => e.ApprovedEmpId, "fk_tbl_idm_cersai_registration_tbl_trg_employee");

                entity.Property(e => e.IdmDsbChargeId)
                    .HasColumnName("idm_dsb_charge_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.SecurityCd)
                    .HasColumnName("security_cd");

                entity.Property(e => e.AssetCd)
                   .HasColumnName("asset_cd");

                entity.Property(e => e.CersaiRegNo)
                                  .HasMaxLength(100)
                  .HasColumnName("cersai_reg_no");

                entity.Property(e => e.CersaiRegDate)
                  .HasColumnName("cersai_reg_date");

                entity.Property(e => e.CersaiRemarks)
                                  .HasMaxLength(500)
                  .HasColumnName("cersai_remarks");

                entity.Property(e => e.UploadDocument)
                                .HasMaxLength(300)
                .HasColumnName("upload_document");

                entity.Property(e => e.ApprovedEmpId)
                                .HasMaxLength(8)
                .HasColumnName("approved_emp_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.ApprovedEmpId)
                     .HasColumnName("approved_emp_id");

                entity.Property(e => e.AssetVal)
                    .HasColumnName("asset_value");

                entity.Property(e => e.AssetDet)
                    .HasColumnName("asset_details");

                entity.HasOne(e => e.TblAssetRefnoDet)
                        .WithMany(b => b.TblIdmCersaiRegistration)
                        .HasForeignKey(b => b.AssetCd);

            });

            /// <summary>
            ///  Author: Manoj; Module: Guarantor; Date:02/08/2022
            /// </summary>
            modelBuilder.Entity<TblAppGuarAssetDet>(entity =>
            {
                entity.HasKey(e => e.AppGuarassetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_guar_asset_det");

                entity.HasIndex(e => e.PromoterCode, "FK_tbl_app_guar_asset_det_prom_cdtab");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");

                entity.HasIndex(e => e.AssetcatCd, "fk_tbl_app_guar_asset_det_tbl_assetcat_cdtab");

                entity.HasIndex(e => e.AssettypeCd, "FK_tbl_app_guar_asset_det_tbl_assettype_cdtab");
                entity.HasIndex(e => e.UtCd, "FK_tbl_app_guar_asset_det_tbl_unit_mast");
                entity.HasIndex(e => e.EgNo, "FK_tbl_app_guar_asset_det_tbl_eg_num_mast");

                entity.Property(e => e.AppGuarassetId)
                    .HasColumnName("app_guarasset_id");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.EgNo)
                    .HasColumnName("eg_no");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.AssetcatCd)
                   .HasColumnName("assetcat_cd");

                entity.Property(e => e.AssettypeCd)
                  .HasColumnName("assettype_cd");

                entity.Property(e => e.LandType)
                            .HasMaxLength(20)
                  .HasColumnName("land_type");

                entity.Property(e => e.AppAssetSiteNo)
                                  .HasMaxLength(50)
                  .HasColumnName("app_asset_siteno");

                entity.Property(e => e.AppAssetAddr)
                                .HasMaxLength(200)
                .HasColumnName("app_asset_addr");

                entity.Property(e => e.AppAssetDim)
                                .HasMaxLength(100)
                .HasColumnName("app_asset_dim");

                entity.Property(e => e.AppAssetArea)
                .HasColumnName("app_asset_area");

                entity.Property(e => e.AppAssetDesc)
                                .HasMaxLength(500)
                .HasColumnName("app_asset_desc");

                entity.Property(e => e.AppAssetvalue)
                                .HasMaxLength(8)
                .HasColumnName("app_asset_value");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                //entity.HasOne(e => e.TblIdmGuarDeedDet)
                //       .WithOne(b => b.TblAppGuarAssetDet)
                //       .HasForeignKey<TblAppGuarAssetDet>(b => b.AppGuarassetId);
            });


            /// <summary>
            ///  Author: Manoj; Module: Guarantor; Date:02/08/2022
            /// </summary>
            modelBuilder.Entity<TblIdmGuarDeedDet>(entity =>
            {
                entity.HasKey(e => e.IdmGuarDeedId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_guar_deed_det");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_guar_deed_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_idm_guar_deed_det_tbl_prom_cdtab");

                entity.HasIndex(e => e.ApprovedEmpId, "fk_tbl_idm_guar_deed_det_tbl_idm_dsb_spl_cleg");
                entity.HasIndex(e => e.AppGuarassetId, "fk_tbl_idm_guar_deed_det_tbl_app_guar_asset_det");

                entity.Property(e => e.IdmGuarDeedId)
                    .HasColumnName("idm_guar_deed_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.AppGuarassetId)
                    .HasColumnName("app_guarasset_id");

                entity.Property(e => e.ValueAsset)
                   .HasColumnName("value_asset");

                entity.Property(e => e.ValueLiab)
                  .HasColumnName("value_liab");

                entity.Property(e => e.ValueNetWorth)
                  .HasColumnName("value_networth");

                entity.Property(e => e.DeedNo)
                                  .HasMaxLength(50)
                  .HasColumnName("deed_no");

                entity.Property(e => e.DeedDesc)
                                .HasMaxLength(200)
                .HasColumnName("deed_desc");

                entity.Property(e => e.ExcecutionDate)
                .HasColumnName("execution_date");

                entity.Property(e => e.DeedUpload)
                                .HasMaxLength(300)
                .HasColumnName("deed_upload");

                entity.Property(e => e.ApprovedEmpId)
                                .HasMaxLength(8)
                .HasColumnName("approved_emp_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.HasOne(e => e.TblAppGuarAssetDet)
                       .WithOne(b => b.TblIdmGuarDeedDet)
                       .HasForeignKey<TblIdmGuarDeedDet>(b => b.AppGuarassetId);

               
            });
            /// <summary>
            ///  Author: Manoj; Module: Guarantor; Date:04/08/2022
            /// </summary>
            modelBuilder.Entity<TblAppGuarnator>(entity =>
            {
                entity.HasKey(e => e.AppGuarId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_guarnator");

                entity.HasIndex(e => e.PromoterCode, "FK_tbl_app_guar_asset_det_prom_cdtab");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");

                entity.HasIndex(e => e.PclasCd, "fk_tbl_app_guarnator_tbl_pclas_cdtab");

                entity.HasIndex(e => e.PsubclasCd, "fk_tbl_app_guarnator_tbl_psubclas_cdtab");
                entity.HasIndex(e => e.DomCd, "fk_tbl_app_guarnator_tbl_domi_cdtab");
                entity.HasIndex(e => e.UtCd, "FK_tbl_app_guar_asset_det_tbl_unit_mast");
                entity.HasIndex(e => e.EgNo, "FK_tbl_app_guar_asset_det_tbl_eg_num_mast");

                entity.Property(e => e.AppGuarId)
                    .HasColumnName("app_guar_id");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.EgNo)
                    .HasColumnName("eg_no");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.UtName)
                .HasMaxLength(100)
                   .HasColumnName("ut_name");

                entity.Property(e => e.GuarName)
                .HasMaxLength(35)
                  .HasColumnName("guar_name");

                entity.Property(e => e.GuarGender)
                  .HasColumnName("guar_gender");

                entity.Property(e => e.GuarDob)
                  .HasColumnName("guar_dob");

                entity.Property(e => e.GuarAge)
                .HasColumnName("guar_age");

                entity.Property(e => e.NameFatherSpouse)
                                .HasMaxLength(150)
                .HasColumnName("name_father_spouse");

                entity.Property(e => e.PclasCd)
                .HasColumnName("pclas_cd");

                entity.Property(e => e.PsubclasCd)
                .HasColumnName("psubclas_cd");

                entity.Property(e => e.DomCd)
                .HasColumnName("dom_cd");

                entity.Property(e => e.GuarPassport)
                               .HasMaxLength(20)
               .HasColumnName("guar_passport");

                entity.Property(e => e.GuarPan)
                               .HasMaxLength(10)
               .HasColumnName("guar_pan");

                entity.Property(e => e.GuarMobileNo)
                               .HasMaxLength(10)
               .HasColumnName("guar_mobile_no");

                entity.Property(e => e.GuarEmail)
                               .HasMaxLength(50)
               .HasColumnName("guar_email");

                entity.Property(e => e.PromTelNo)
                               .HasMaxLength(8)
               .HasColumnName("prom_tel_no");

                entity.Property(e => e.PromPhoto)
                               .HasMaxLength(300)
               .HasColumnName("prom_photo");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Manoj; Module: Guarantor; Date:05/08/2022
            /// </summary>
            modelBuilder.Entity<TblAppGuarLiabDet>(entity =>
            {
                entity.HasKey(e => e.AppGuarLiabId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_guar_liab_det");

                entity.HasIndex(e => e.PromoterCode, "FK_tbl_app_guar_asset_det_prom_cdtab");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");
                entity.HasIndex(e => e.UtCd, "FK_tbl_app_guar_asset_det_tbl_unit_mast");
                entity.HasIndex(e => e.EgNo, "FK_tbl_app_guar_asset_det_tbl_eg_num_mast");

                entity.Property(e => e.AppGuarLiabId)
                    .HasColumnName("app_guarliab_id");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.EgNo)
                    .HasColumnName("eg_no");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.AppLiabDesc)
                .HasMaxLength(500)
                   .HasColumnName("app_liab_desc");

                entity.Property(e => e.AppLiabValue)
                  .HasColumnName("app_liab_value");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Manoj; Module: Guarantor; Date:05/08/2022
            /// </summary>
            modelBuilder.Entity<TblAppGuarNwDet>(entity =>
            {
                entity.HasKey(e => e.AppGuarNwId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_app_guar_nw_det");

                entity.HasIndex(e => e.PromoterCode, "FK_tbl_app_guar_asset_det_prom_cdtab");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");
                entity.HasIndex(e => e.UtCd, "FK_tbl_app_guar_asset_det_tbl_unit_mast");
                entity.HasIndex(e => e.EgNo, "FK_tbl_app_guar_asset_det_tbl_eg_num_mast");

                entity.Property(e => e.AppGuarNwId)
                    .HasColumnName("app_guar_nw_id");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.EgNo)
                    .HasColumnName("eg_no");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.AppImmov)
                   .HasColumnName("app_immov");

                entity.Property(e => e.AppMov)
                  .HasColumnName("app_mov");

                entity.Property(e => e.AppLiab)
                  .HasColumnName("app_liab");

                entity.Property(e => e.AppNw)
                  .HasColumnName("app_nw");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            modelBuilder.Entity<TblChargeType>(entity =>
            {
                entity.HasKey(e => e.Id)
                                    .HasName("PRIMARY");

                entity.ToTable("tbl_charge_type");

                entity.Property(e => e.Id)
                                   .HasColumnName("id");

                entity.Property(e => e.ChargeType)
                                    .HasMaxLength(45)
                                    .HasColumnName("chargetype");

                entity.Property(e => e.IsActive)
               .HasDefaultValue(true)
               .HasColumnName("is_active");


            });

            /// <summary>
            ///  Author: Manoj; Module: Guarantor; Date:05/08/2022
            /// </summary>
            modelBuilder.Entity<TblAppGuarNwDet>(entity =>
            {
                entity.HasKey(e => e.AppGuarNwId)
                                  .HasName("PRIMARY");

                entity.ToTable("tbl_app_guar_nw_det");

                entity.HasIndex(e => e.PromoterCode, "FK_tbl_app_guar_asset_det_prom_cdtab");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_cersai_registration_offc_cdtab");
                entity.HasIndex(e => e.UtCd, "FK_tbl_app_guar_asset_det_tbl_unit_mast");
                entity.HasIndex(e => e.EgNo, "FK_tbl_app_guar_asset_det_tbl_eg_num_mast");

                entity.Property(e => e.AppGuarNwId)
                                  .HasColumnName("app_guar_nw_id");

                entity.Property(e => e.PromoterCode)
                                  .HasColumnName("promoter_code");

                entity.Property(e => e.EgNo)
                                  .HasColumnName("eg_no");

                entity.Property(e => e.OffcCd)
                                .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                                  .HasColumnName("ut_cd");

                entity.Property(e => e.AppImmov)
                                 .HasColumnName("app_immov");

                entity.Property(e => e.AppMov)
                                .HasColumnName("app_mov");

                entity.Property(e => e.AppLiab)
                                .HasColumnName("app_liab");

                entity.Property(e => e.AppNw)
                                .HasColumnName("app_nw");

                entity.Property(e => e.IsActive)
                                 .HasDefaultValue(true)
                                  .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                  .HasDefaultValue(false)
                                  .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                  .HasMaxLength(50)
                                  .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                  .HasMaxLength(50)
                                  .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                              .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                  .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Akhiladevi D M; Module: Condition; Update Date:06/08/2022
            ///  Updated by Gagana
            /// </summary>
            modelBuilder.Entity<TblIdmCondDet>(entity =>
            {
                entity.HasKey(e => e.CondDetId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_cond_det");

                entity.HasIndex(e => e.LoanAcc, "tbl_idm_cond_det_ibfk_1");

                entity.HasIndex(e => e.OffcCd, "tbl_idm_cond_det_ibfk_2");

                entity.HasIndex(e => e.CondType, "tbl_idm_cond_det_ibfk_3");

                entity.HasIndex(e => e.CondStg, "tbl_idm_cond_det_ibfk_4");

                entity.Property(e => e.CondDetId)
                    .HasColumnName("cond_det_id");

                entity.Property(e => e.LoanAcc)
                        .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                        .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                        .HasColumnName("offc_cd");

                entity.Property(e => e.CondType)
                        .HasColumnName("cond_type");

                entity.Property(e => e.CondCd)
                        .HasColumnName("cond_cd");

                entity.Property(e => e.CondStg)
                       .HasColumnName("cond_stg");

                entity.Property(e => e.CondDetails)
                                        .HasMaxLength(200)
                      .HasColumnName("cond_details");

                entity.Property(e => e.Compliance)
                        .HasMaxLength(5)
                        .HasColumnName("cond_compl");

                entity.Property(e => e.CondRemarks)
                                        .HasMaxLength(200)
                      .HasColumnName("cond_remarks");

                entity.Property(e => e.WhRelaxation)
                                      .HasMaxLength(1)
                      .HasColumnName("wh_relaxation");

                entity.Property(e => e.WhRelAllowed)
                                      .HasMaxLength(1)
                      .HasColumnName("wh_rel_sought");

                entity.Property(e => e.CondUpload)
                                    .HasMaxLength(100)
                    .HasColumnName("cond_upload");

                entity.Property(e => e.IsActive)
                                   .HasDefaultValue(true)
                                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.HasOne(e => e.TblCondStageMast)
                        .WithOne(b => b.TblIdmCondDet)
                        .HasForeignKey<TblIdmCondDet>(b => b.CondStg);

                entity.HasOne(e => e.TblCondTypeCdtab)
                      .WithOne(b => b.TblIdmCondDet)
                      .HasForeignKey<TblIdmCondDet>(b => b.CondType);

            });


            modelBuilder.Entity<TblChargeType>(entity =>
            {
                entity.HasKey(e => e.Id)
                                    .HasName("PRIMARY");

                entity.ToTable("tbl_charge_type_cdtab");

                entity.Property(e => e.Id)
                                   .HasColumnName("chrg_type_cd");

                entity.Property(e => e.ChargeType)
                                    .HasMaxLength(150)
                                    .HasColumnName("chrg_type_dets");

                entity.Property(e => e.DisplaySequence)
                .HasColumnName("chrg_type_dis_seq");

                entity.Property(e => e.IsActive)
                                   .HasDefaultValue(true)
                                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                     .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });
            /// <summary>
            ///  Author: Gagana; Module: Condition; Update Date:10/08/2022
            /// </summary>
            modelBuilder.Entity<TblCondStgCdtab>(entity =>
            {
                entity.HasKey(e => e.CondStgCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cond_stg_cdtab");

                entity.Property(e => e.CondStgCd)
                    .HasColumnName("cond_stg_cd");

                entity.Property(e => e.CondStgDets)
                    .HasColumnName("cond_stg_dets");

                entity.Property(e => e.CondStgDisSeq)
                    .HasColumnName("cond_stg_dis_seq");

                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Gagana; Module: Condition; Update Date:10/08/2022
            /// </summary>

            modelBuilder.Entity<TblCondTypeCdtab>(entity =>
            {
                entity.HasKey(e => e.CondTypeCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cond_type_cdtab");

                entity.Property(e => e.CondTypeCd)
                    .HasColumnName("cond_type_cd");

                entity.Property(e => e.CondTypeDets)
                    .HasColumnName("cond_type_dets");

                entity.Property(e => e.CondTypeDisSeq)
                    .HasColumnName("cond_type_dis_seq");

                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Gagana; Module: Condition; Update Date:10/08/2022
            /// </summary>

            modelBuilder.Entity<TblCondDetCdtab>(entity =>
            {
                entity.HasKey(e => e.CondDetCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cond_det_cdtab");

                entity.Property(e => e.CondDetCd)
                    .HasColumnName("cond_det_cd");

                entity.Property(e => e.CondDets)
                    .HasColumnName("cond_det_dets");

                entity.Property(e => e.CondDetDisSeq)
                    .HasColumnName("cond_det_dis_seq");

                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });

            modelBuilder.Entity<TblCondStageMast>(entity =>
            {
                entity.HasKey(e => e.CondStageId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cond_stage_mast");

                entity.Property(e => e.CondStageId)
                 .HasColumnName("condstage_id");

                entity.Property(e => e.CondStg)
                    .HasColumnName("cond_stg");

                entity.Property(e => e.CondStgDets)
                    .HasColumnName("cond_stg_dets");

                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });

            modelBuilder.Entity<TblAddlCondDet>(entity =>
            {
                entity.HasKey(e => e.AddCondId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_addlcond_det");

                entity.HasIndex(e => e.LoanAcc, name: "tbl_idm_addlcond_det_ibfk_1");
                entity.HasIndex(e => e.OffcCd, "tbl_idm_addlcond_det_ibfk_2");
                entity.HasIndex(e => e.AddCondStage, "tbl_idm_addlcond_det_ibfk_3");

                entity.Property(e => e.AddCondId)
                    .HasColumnName("addlcond_det_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");


                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.AddCondCode)
                    .HasColumnName("addl_cond_code");

                entity.Property(e => e.AddCondStage)
                    .HasColumnName("addl_cond_stg");

                entity.Property(e => e.AddCondDetails)
                         .HasMaxLength(200)
                    .HasColumnName("addl_cond_details");

                entity.Property(e => e.Relaxation)
                        .HasMaxLength(1)
                    .HasColumnName("wh_relaxation");

                entity.Property(e => e.Compliance)
                        .HasMaxLength(5)
                   .HasColumnName("addcond_compl");


                entity.Property(e => e.WhRelAllowed)
                                .HasMaxLength(1)
                      .HasColumnName("wh_rel_sought");

                entity.Property(e => e.IsActive)
                                   .HasDefaultValue(true)
                                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                     .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

                entity.HasOne(e => e.TblCondStgCdtab)
                     .WithOne(b => b.TblAddlCondDet)
                     .HasForeignKey<TblAddlCondDet>(b => b.AddCondStage);


            });

            modelBuilder.Entity<TblCondStgMast>(entity =>
            {
                entity.HasKey(e => e.CondStgId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_cond_stg_mast");

                entity.Property(e => e.CondStgId)
                    .HasColumnName("cond_stag_id");

                entity.Property(e => e.CondStgCode)
                    .HasColumnName("cond_stag_code");

                entity.Property(e => e.CondStgDesc)
                      .HasMaxLength(100)
                    .HasColumnName("cond_stag_desc");

            });

            /// <summary>
            ///  Author: Gagana K; Module: Audit; Date:18/08/2022
            /// </summary>
            modelBuilder.Entity<TblIdmAuditDet>(entity =>
            {
                entity.HasKey(e => e.IdmAuditId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_audit_det");

                entity.HasIndex(e => e.LoanAcc, "tbl_idm_audit_det_ibfk_1");

                entity.HasIndex(e => e.OffcCd, "tbl_idm_audit_det_ibfk_2");

                entity.Property(e => e.IdmAuditId)
                    .HasColumnName("idm_audit_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.AuditObservation)
                                  .HasMaxLength(150)
                  .HasColumnName("audit_observation");

                entity.Property(e => e.AuditCompliance)
                                  .HasMaxLength(150)
                  .HasColumnName("audit_compliance");

                entity.Property(e => e.AuditUpload)
                                .HasMaxLength(100)
                .HasColumnName("audit_upload");

                entity.Property(e => e.AuditEmpId)
                 .HasColumnName("audit_emp_id");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Manoj; Module: Form8AndForm13; Date:19/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDsbFm813>(entity =>
            {
                entity.HasKey(e => e.DF813Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dsb_fm813");

                entity.HasIndex(e => e.DF813Offc, "tbl_idm_dsb_fm813_ibfk_1");

                entity.HasIndex(e => e.DF813Unit, "tbl_idm_dsb_fm813_ibfk_2");

                entity.HasIndex(e => e.DF813t1, "tbl_idm_dsb_fm813_ibfk_3");

                entity.Property(e => e.DF813Id)
                    .HasColumnName("df813_id");

                entity.Property(e => e.DF813Offc)
                    .HasColumnName("df813_offc");

                entity.Property(e => e.DF813Unit)
                    .HasColumnName("df813_unit");

                entity.Property(e => e.DF813Sno)
                    .HasColumnName("df813_sno");

                entity.Property(e => e.DF813Dt)
                   .HasColumnName("df813_dt");

               entity.Property(e => e.DF813RqDt)
                   .HasColumnName("df813_rqdt");

                entity.Property(e => e.DF813Ref)
                .HasMaxLength(100)
                   .HasColumnName("df813_ref");

                entity.Property(e => e.DF813cc)
                   .HasColumnName("df813_cc");

                entity.Property(e => e.DF813t1)
                  .HasColumnName("df813_t1");

                entity.Property(e => e.DF813a1)
                  .HasColumnName("df813_a1");

                entity.Property(e => e.DF813Upload)
                  .HasColumnName("df813_upload");

                entity.Property(e => e.DF813LoanAcc)
                  .HasColumnName("df813_loan_acc");

                entity.Property(e => e.IsActive)
                                 .HasDefaultValue(true)
                                  .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.HasOne(e => e.Tblfm8fm13CDTab)
                      .WithOne(b => b.TblIdmDsbFm813)
                      .HasForeignKey<TblIdmDsbFm813>(b => b.DF813t1);
            });

            /// <summary> 
            /// Author: Dev; Module: Sidbi; Date: 19/08/2022
            /// </summary>
            modelBuilder.Entity<TblIdmSidbiApproval>(entity =>
            {
                entity.HasKey(e => e.SidbiApprId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_sidbi_approval");

                entity.HasIndex(e => e.LoanAcc, "tbl_idm_sidbi_approval_ibfk_1");

                entity.HasIndex(e => e.OffcCd, "tbl_idm_sidbi_approval_ibfk_2");

                entity.HasIndex(e => e.PromTypeCd, "tbl_idm_sidbi_approval_ibfk_3");

                entity.Property(e => e.SidbiApprId)
                    .HasColumnName("sidbi_appr_id");

                entity.Property(e => e.PromTypeCd)
                    .HasColumnName("prom_type_cd");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.LnSancAmt)
                    .HasColumnName("ln_sanc_amt");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.WhAppr)
                    .HasColumnName("wh_appr");

                entity.Property(e => e.SidbiUpload)
                    .HasMaxLength(100)
                    .HasColumnName("sidbi_upload");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.ModifiedBy)
                                   .HasMaxLength(50)
                                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");


                entity.HasOne(e => e.TblPromTypeCdtab)
                        .WithOne(b => b.TblIdmSidbiApproval)
                        .HasForeignKey<TblIdmSidbiApproval>(b => b.PromTypeCd);

            });

            modelBuilder.Entity<TblPromTypeCdtab>(entity =>
            {
                entity.HasKey(e => e.PromTypeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_prom_type_cdtab");

                entity.Property(e => e.PromTypeId)
                    .HasColumnName("prom_type_cd");

                entity.Property(e => e.PromTypeDets)
                    .HasColumnName("prom_type_dets");

                entity.Property(e => e.PromTypeDisSeq)
                    .HasColumnName("prom_type_dis_seq");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");


            });
            /// <summary>
            ///  Author: Akhiladevi DM ; Module: F; Date:19/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmFirstInvestmentClause>(entity =>
            {
                entity.HasKey(e => e.DCFICId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_fic");

                entity.HasIndex(e => e.DCFICOffc, "tbl_idm_dchg_fic_ibfk_1");

                entity.HasIndex(e => e.DCFICLoanACC, "tbl_idm_dchg_fic_ibfk_2");

                entity.Property(e => e.DCFICId)
                    .HasColumnName("dcfic_id");

                entity.Property(e => e.DCFICOffc)
                    .HasColumnName("dcfic_offc");

                entity.Property(e => e.DCFICLoanACC)
                    .HasColumnName("dcfic_loan_no");

                entity.Property(e => e.DCFICSno)
                    .HasColumnName("dcfic_sno");

                entity.Property(e => e.DCFICRequestDate)
                    .HasColumnName("dcfic_rqdt");

                entity.Property(e => e.DCFICApproveDate)
                    .HasColumnName("dcfic_apdt");

                entity.Property(e => e.DCFICApproveAUDate)
                    .HasColumnName("dcfic_apau");

                entity.Property(e => e.DCFICAmount)
                    .HasColumnName("dcfic_amt");

                entity.Property(e => e.DCFICCommunicationDate)
                    .HasColumnName("dcfic_comdt");

                entity.Property(e => e.DCFICAmountOriginal)
                    .HasColumnName("dcfic_amt_original");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author: Gagana K ; Module: Change Location Details; Date:19/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmUnitAddress>(entity =>
            {

                entity.HasKey(e => e.IdmUtAddressRowid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_unit_address");

                entity.HasIndex(e => e.AddtypeCd, "fK_tbl_idm_unit_address_tbl_address_cdtab");

                entity.HasIndex(e => e.UtCd, "fK_tbl_idm_unit_address_tbl_app_unit_details");

                entity.HasIndex(e => e.UtDistCd, "fk_tbl_idm_unit_address_dist_cdtab");

                entity.HasIndex(e => e.UtHobCd, "fk_tbl_idm_unit_address_hob_cdtab");

                entity.HasIndex(e => e.UtTlqCd, "fk_tbl_idm_unit_address_tlq_cdtab");

                entity.HasIndex(e => e.UtVilCd, "fk_tbl_idm_unit_address_vil_cdtab");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_unit_address_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_unit_address_offc_cdtab");

                entity.Property(e => e.IdmUtAddressRowid)
                  .HasColumnName("idm_utaddress_rowid");

                entity.Property(e => e.LoanAcc)
                  .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd).HasColumnName("ut_cd");

                entity.Property(e => e.AddtypeCd).HasColumnName("addtype_cd");

                entity.Property(e => e.UtAddress)
                    .HasMaxLength(200)
                    .HasColumnName("ut_address");

                entity.Property(e => e.UtArea)
                   .HasMaxLength(100)
                   .HasColumnName("ut_area");

                entity.Property(e => e.UtCity)
                    .HasMaxLength(100)
                    .HasColumnName("ut_city");

                entity.Property(e => e.UtPincode).HasColumnName("ut_pincode");

                entity.Property(e => e.UtTelephone).HasColumnName("ut_telephone");

                entity.Property(e => e.UtMobile).HasColumnName("ut_mobile");

                entity.Property(e => e.UtAltMobile).HasColumnName("ut_alt_mobile");

                entity.Property(e => e.UtEmail)
                    .HasMaxLength(50)
                    .HasColumnName("ut_email");

                entity.Property(e => e.UtAltEmail)
                   .HasMaxLength(50)
                   .HasColumnName("ut_alt_email");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate).HasColumnName("modified_date");

                entity.Property(e => e.UtDistCd).HasColumnName("ut_dist_cd");
                entity.Property(e => e.UtTlqCd).HasColumnName("ut_tlq_cd");
                entity.Property(e => e.UtHobCd).HasColumnName("ut_hob_cd");
                entity.Property(e => e.UtVilCd).HasColumnName("ut_vil_cd");

                entity.HasOne(e => e.TblAddressCdtab)
                   .WithOne(b => b.TblIdmUnitAddress)
                   .HasForeignKey<TblIdmUnitAddress>(b => b.AddtypeCd);

            });

            // Dev
            modelBuilder.Entity<IdmPromoter>(entity =>
            {
                entity.HasKey(e => e.IdmPromId)
                    .HasName("PRIMARY");

                entity.ToTable("idm_promoter");

                entity.HasIndex(e => e.LoanAcc, "idm_promoter_ibfk_1");

                entity.HasIndex(e => e.OffcCd, "idm_promoter_ibfk_2");

                entity.HasIndex(e => e.UtCd, "idm_promoter_ibfk_3");

                entity.HasIndex(e => e.PdesigCd, "idm_promoter_ibfk_5");

                entity.Property(e => e.IdmPromId)
                  .HasColumnName("idm_prom_id");

                entity.Property(e => e.LoanAcc)
                  .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                  .HasColumnName("ut_cd");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.PromName)
                    .HasColumnName("prom_name");

                entity.Property(e => e.PdesigCd)
                  .HasColumnName("pdesig_cd");

                entity.Property(e => e.PromGender)
                    .HasColumnName("prom_gender");

                entity.Property(e => e.PromDob)
                    .HasColumnName("prom_dob");

                entity.Property(e => e.PromAge)
                  .HasColumnName("prom_age");

                entity.Property(e => e.NameFatherSpouse)
                    .HasColumnName("name_father_spouse");

                entity.Property(e => e.PclasCd)
                    .HasColumnName("pclas_cd");

                entity.Property(e => e.PsubclasCd)
                  .HasColumnName("psubclas_cd");

                entity.Property(e => e.PromExpYrs)
                    .HasColumnName("prom_exp_yrs");

                entity.Property(e => e.PromExpDet)
                    .HasColumnName("prom_exp_det");

                entity.Property(e => e.PqualCd)
                  .HasColumnName("pqual_cd");

                entity.Property(e => e.PromAddlQual)
                    .HasColumnName("prom_addlqual");

                entity.Property(e => e.DomCd)
                    .HasColumnName("dom_cd");

                entity.Property(e => e.PromPassport)
                  .HasColumnName("prom_passport");

                entity.Property(e => e.PromPan)
                    .HasColumnName("prom_pan");

                entity.Property(e => e.PromJnDt)
                    .HasColumnName("prom_jn_dt");

                entity.Property(e => e.PromExDt)
                  .HasColumnName("prom_ex_dt");

                entity.Property(e => e.PromMobileNo)
                    .HasColumnName("prom_mobile_no");

                entity.Property(e => e.PromEmail)
                    .HasColumnName("prom_email");

                entity.Property(e => e.PromTelNo)
                 .HasColumnName("prom_tel_no");

                entity.Property(e => e.PromChief)
                    .HasColumnName("prom_chief");

                entity.Property(e => e.PromMajor)
                    .HasColumnName("prom_major");

                entity.Property(e => e.PromPhyHandicap)
                 .HasColumnName("prom_phy_handicap");

                entity.Property(e => e.PromPhoto)
                    .HasColumnName("prom_photo");

                entity.Property(e => e.IsActive)
                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                   .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.HasOne(e => e.TblPdesigCdtab)
                   .WithOne(b => b.IdmPromoter)
                   .HasForeignKey<IdmPromoter>(b => b.PdesigCd);

            });

            //Added by GK
            modelBuilder.Entity<TblIdmPromAddress>(entity =>
            {
                entity.HasKey(e => e.IdmPromadrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_prom_address");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_prom_address_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_prom_address_offc_cdtab");

                entity.HasIndex(e => e.PromDistrictCd, "fk_tbl_idm_prom_address_tbl_pincode_district_cdtab");
                entity.HasIndex(e => e.PromStateCd, "fk_tbl_idm_prom_address_tbl_pincode_state_cdtab");
              

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_idm_prom_address_tbl_prom_cdtab");


                entity.Property(e => e.IdmPromadrId)
                    .HasColumnName("idm_promadr_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.PromAddress)
                                  .HasMaxLength(500)
                  .HasColumnName("prom_address");

                entity.Property(e => e.PromStateCd)
                    .HasColumnName("prom_state_cd");

                entity.Property(e => e.PromDistrictCd)
                    .HasColumnName("prom_district_cd");

                entity.Property(e => e.PromPincode)
                 .HasColumnName("prom_pincode");

                entity.Property(e => e.AdrPermanent)
                   .HasDefaultValue(true)
                   .HasColumnName("adr_permanent");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.HasOne(e => e.TblPincodeDistrictCdtab)
                    .WithOne(b => b.TblIdmPromAddress)
                    .HasForeignKey<TblIdmPromAddress>(b => b.PromDistrictCd);

                entity.HasOne(e => e.TblPincodeStateCdtab)
                   .WithOne(b => b.TblIdmPromAddress)
                   .HasForeignKey<TblIdmPromAddress>(b => b.PromStateCd);

                entity.HasOne(e => e.TblPincodeMaster)
                 .WithOne(b => b.TblIdmPromAddress)
                 .HasForeignKey<TblIdmPromAddress>(b => b.PromPincode);

            });

            //Added by GK
            modelBuilder.Entity<TblPincodeDistrictCdtab>(entity =>
            {
                entity.HasKey(e => e.PincodeDistrictCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pincode_district_cdtab");

                entity.HasIndex(e => e.PincodeStateCd, "fk_tbl_pincode_district_cdtab_tbl_pincode_state_cdtab");

                entity.Property(e => e.PincodeDistrictCd)
                    .HasColumnName("pincode_district_cd");

                entity.Property(e => e.PincodeDistrictDesc)
                     .HasMaxLength(200)
                    .HasColumnName("pincode_district_desc");

                entity.Property(e => e.PincodeStateCd)
                    .HasColumnName("pincode_state_cd");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.ModifiedBy)
                 .HasMaxLength(50)
                 .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.Distcd)
                .HasColumnName("DIST_CD");

                entity.HasOne(e => e.PincodeStateCdtab)
                 .WithOne(b => b.TblPincodeDistrictCdtab)
                 .HasForeignKey<TblPincodeDistrictCdtab>(b => b.PincodeStateCd);
            });

            modelBuilder.Entity<TblPincodeStateCdtab>(entity =>
            {
                entity.HasKey(e => e.PincodeStateCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pincode_state_cdtab");

                entity.Property(e => e.PincodeStateCd)
                    .HasColumnName("pincode_state_cd");

                entity.Property(e => e.PincodeStateDesc)
                    .HasMaxLength(200)
                    .HasColumnName("Pincode_state_desc");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            // Dev
            modelBuilder.Entity<IdmPromoterBankDetails>(entity =>
            {
                entity.HasKey(e => e.IdmPromBankId)
                    .HasName("PRIMARY");

                entity.ToTable("idm_promoter_bank_details");

                entity.HasIndex(e => e.LoanAcc, "idm_promoter_bank_details_ibfk_1");

                entity.HasIndex(e => e.OffcCd, "idm_promoter_bank_details_ibfk_2");

                entity.HasIndex(e => e.PrmIfscId, "idm_promoter_bank_details_ibfk_5");


                entity.Property(e => e.IdmPromBankId)
                  .HasColumnName("idm_prom_bank_id");

                entity.Property(e => e.LoanAcc)
                  .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                  .HasColumnName("ut_cd");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.PrmAcType)
                    .HasColumnName("prm_ac_type");

                entity.Property(e => e.PrmBankName)
                  .HasColumnName("prm_bank_name");

                entity.Property(e => e.PrmBankBranch)
                    .HasColumnName("prm_bank_branch");

                entity.Property(e => e.PrmBankLoc)
                    .HasColumnName("prm_bank_loc");

                entity.Property(e => e.PrmAcNo)
                  .HasColumnName("prm_ac_no");

                entity.Property(e => e.PrmIfscId)
                    .HasColumnName("prm_ifsc_id");

                entity.Property(e => e.PrmBankAddress)
                    .HasColumnName("prm_bank_address");

                entity.Property(e => e.PrmBankState)
                  .HasColumnName("prm_bank_state");

                entity.Property(e => e.PrmBankDistrict)
                    .HasColumnName("prm_bank_district");

                entity.Property(e => e.PrmBankTaluk)
                  .HasColumnName("prm_bank_taluka");

                entity.Property(e => e.PrmBankPincode)
                    .HasColumnName("prm_bank_pincode");

                entity.Property(e => e.PrmBankAcName)
                    .HasColumnName("prm_bank_ac_name");

                entity.Property(e => e.PrmPrimaryBank)
                  .HasColumnName("prm_primary_bank");

                entity.Property(e => e.PrmCibilScore)
                    .HasColumnName("prm_cibil_score");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");
                //entity.HasOne(e => e.TblPromCdtab)
                //    .WithOne(b => b.IdmPromoterBankDetails)
                //    .HasForeignKey<IdmPromoterBankDetails>(b => b.PromoterCode);

                entity.HasOne(e => e.TbIIfscMaster)
                   .WithOne(b => b.IdmPromoterBankDetails)
                   .HasForeignKey<IdmPromoterBankDetails>(b => b.PrmIfscId);

            });

            modelBuilder.Entity<TblPsubclasCdtab>(entity =>
            {
                entity.HasKey(e => e.PsubclasCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_psubclas_cdtab");

                entity.Property(e => e.PsubclasCd)
                    .HasColumnName("psubclas_cd");

                entity.Property(e => e.PsubclasDesc)
                    .HasMaxLength(200)
                    .HasColumnName("psubclas_desc");

                entity.Property(e => e.PclasCd)
                    .HasColumnName("pclas_cd");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            modelBuilder.Entity<TblPqualCdtab>(entity =>
            {
                entity.HasKey(e => e.PqualCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pqual_cdtab");

                entity.Property(e => e.PqualDesc)
                .HasColumnName("pqual_desc");
                entity.Property(e => e.PqualCd)
                    .HasColumnName("pqual_cd");
                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

            });

            /// <summary>
            ///  Author: Manoj ; Module: Land Inspection; Date:25/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDchgLand>(entity =>
            {
                entity.HasKey(e => e.DcLndRowId)
                    .HasName("PRIMARY");
                entity.ToTable("tbl_idm_dchg_land");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_land_tbl_app_loan_mast");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_land_offc_cdtab");

                entity.Property(e => e.DcLndRowId)
                    .HasColumnName("dclnd_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                  .HasColumnName("offc_cd");

                entity.Property(e => e.DcLndArea)
                  .HasColumnName("dclnd_area");

                entity.Property(e => e.DcLndAreaIn)
                .HasMaxLength(10)
                    .HasColumnName("dclnd_areain");

                entity.Property(e => e.DcLndType)
                    .HasColumnName("dclnd_type");

                entity.Property(e => e.DcLndAmt)
                    .HasColumnName("dclnd_amt");

                entity.Property(e => e.DcLndApau)
                    .HasColumnName("dclnd_apau");

                entity.Property(e => e.DcLndApdt)
                    .HasColumnName("dclnd_apdt");

                entity.Property(e => e.DcLndDevCst)
                    .HasColumnName("dclnd_devcst");

                entity.Property(e => e.DcLndLandFinance)
                    .HasColumnName("dclnd_land_finance");

                entity.Property(e => e.DcLndStatChgDate)
                    .HasColumnName("dclnd_stat_chgdate");

                entity.Property(e => e.DcLndAqrdIndicator)
                   .HasColumnName("dclnd_aqrd_indicator");

                entity.Property(e => e.DcLndSecCreated)
                   .HasColumnName("dclnd_sec_created");

                entity.Property(e => e.DcLndIno)
                   .HasColumnName("dclnd_ino");

                 entity.Property(e => e.DcLndrefNo)
                   .HasColumnName("dclnd_refno");

                entity.Property(e => e.DcLndDets)
                .HasMaxLength(200)
                   .HasColumnName("dclnd_dets");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)
                  .HasDefaultValue(true)
                  .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");
            });
            /// <summary>
            ///  Author: Srinivas  ; Module:Status of implementation; Date:24/04/2023
            /// </summary>
            modelBuilder.Entity<TblDsbStatImp>(entity =>
            {
                entity.HasKey(e => e.DsbId)
                    .HasName("PRIMARY");
                entity.ToTable("tbl_dsb_stat_imp");


                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                   .HasColumnName("offc_cd");

                entity.Property(e => e.DsbId)
                   .HasColumnName("dsb_id");

                entity.Property(e => e.DsbOffc)
                    .HasColumnName("dsb_offc");

                entity.Property(e => e.DsbUnit)
                   .HasColumnName("dsb_unit");

                entity.Property(e => e.DsbSno)
                  .HasColumnName("dsb_sno");

                entity.Property(e => e.DsbIno)
                  .HasColumnName("dsb_ino");

                entity.Property(e => e.DsbImpStat)
                .HasMaxLength(750)
                    .HasColumnName("dsb_imp_stat");

                entity.Property(e => e.DsbNamePl)
                    .HasColumnName("dsb_name_pl");

                entity.Property(e => e.DsbProgimpBldg)
                .HasMaxLength(100)
                   .HasColumnName("dsb_progimp_bldg");

                entity.Property(e => e.DsbProgimpMc)
                .HasMaxLength(100)
                  .HasColumnName("dsb_progimp_mc");

                entity.Property(e => e.DsbBldgVal)
                  .HasColumnName("dsb_bldg_val");


                entity.Property(e => e.DsbMcVal)
                    .HasColumnName("dsb_mc_val");

                entity.Property(e => e.DsbPhyPrg)
                    .HasColumnName("dsb_phy_prg");

                entity.Property(e => e.DsbValPrg)
                   .HasColumnName("dsb_val_prg");

                entity.Property(e => e.DsbTmcstOvr)
                .HasMaxLength(300)
                  .HasColumnName("dsb_tmcst_ovr");

                entity.Property(e => e.DsbRec)
                .HasMaxLength(2000)
                  .HasColumnName("dsb_rec");

                entity.Property(e => e.DsbComplDt)
                    .HasColumnName("dsb_compl_dt");

                entity.Property(e => e.DsbBalBldg)
                .HasMaxLength(750)
                    .HasColumnName("dsb_bal_bldg");

                entity.Property(e => e.CreatedBy)
                   .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                  .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                 .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.IsActive)
                  .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                  .HasColumnName("is_deleted");

                entity.Property(e => e.UniqueId)
                              .HasMaxLength(150)
                  .HasColumnName("unique_id");

            });


                /// <summary>
                ///  Author: Swetha ; Module: Building Inspection; Date:25/08/2022
                /// </summary>

                modelBuilder.Entity<TblIdmDchgBuildingDet>(entity =>
            {
                entity.HasKey(e => e.DcBdgRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_bldg");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_bldg_tbl_app_loan_mast");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_bldg_offc_cdtab");

                entity.Property(e => e.DcBdgRowId)
                    .HasColumnName("dcbdg_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");
                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");
                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DcBdgItmNo)

                    .HasColumnName("dcbdg_itmno");

                entity.Property(e => e.DcBdgDets)
                   .HasMaxLength(600)
                    .HasColumnName("dcbdg_dets");

                entity.Property(e => e.DcBdgRoof)
                    .HasMaxLength(40)
                    .HasColumnName("dcbdg_roof");

                entity.Property(e => e.DcBdgPlnth)
                    .HasColumnName("dcbdg_plnth");

                entity.Property(e => e.DcBdgUcost)
                    .HasColumnName("dcbdg_ucost");

                entity.Property(e => e.DcBdgTcost)

                    .HasColumnName("dcbdg_tcost");

                entity.Property(e => e.DcBdgRqdt)

                    .HasColumnName("dcbdg_rqdt");

                entity.Property(e => e.DcBdgApdt)

                   .HasColumnName("dcbdg_apdt");

                entity.Property(e => e.DcBdgApau)

                   .HasColumnName("dcbdg_apau");

                entity.Property(e => e.DcBdgRqrdStat)
                   .HasColumnName("dcbdg_rqrd_stat");

                entity.Property(e => e.DcBdgComdt)
                .HasColumnName("dcbdg_comdt");

                entity.Property(e => e.DcBdgStat)
                .HasColumnName("dcbdg_stat");

                entity.Property(e => e.DcBdgStatChgDate)
                .HasColumnName("dcbdg_stat_chgdate");

                entity.Property(e => e.DcBdgSecCreatd)
                .HasColumnName("dcbdg_sec_creatd");

                entity.Property(e => e.DcBdgAplnth)
                .HasColumnName("dcbdg_aplnth");

                entity.Property(e => e.DcBdgAtCost)
               .HasColumnName("dcbdg_atcost");

                entity.Property(e => e.DcBdgPercent)
               .HasColumnName("dcbdg_percent");

                entity.Property(e => e.DcBdgIno)
               .HasColumnName("dcbdg_ino");
                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)

                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)

                    .HasColumnName("is_deleted");
            });

            // <summary>
            ///  Author: Srinivas  ; Module: AssetDetails; Date:16/05/2023
            /// </summary>


            modelBuilder.Entity<TblIdmProjland>(entity =>
            {

                entity.HasKey(e => e.PjLandRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_proj_land");

                entity.HasIndex(e => e.PjLandOffc, "fk_tbl_idm_proj_land_offc_cdtab");
                entity.HasIndex(e => e.PjLandUnit, "fk_tbl_idm_proj_land_tbl_app_loan_mast");

                entity.Property(e => e.PjLandRowId)
                .HasColumnName("pjlnd_rowid");

                entity.Property(e => e.UtCd)
                 .HasColumnName("ut_cd");

                entity.Property(e => e.PjLandOffc)
                .HasColumnName("pjlnd_offc");

                entity.Property(e => e.PjLandUnit)
                .HasColumnName("pjlnd_unit");

                entity.Property(e => e.PjLandSno)
                .HasColumnName("pjlnd_sno");

                entity.Property(e => e.PjLandArea)
                .HasColumnName("pjlnd_area");

                entity.Property(e => e.PjLandAreaIn)
                .HasMaxLength(20)
               .HasColumnName("pjlnd_areain");

                entity.Property(e => e.PjLandType)
               .HasColumnName("pjlnd_type");

                entity.Property(e => e.PjLandCost)
               .HasColumnName("pjlnd_cost");

                entity.Property(e => e.PjLandSubArea)
              .HasColumnName("pjlnd_subarea");

                entity.Property(e => e.PjLandAreaUnit)
              .HasColumnName("pjlnd_areaunit");

                entity.Property(e => e.PjLandSubAreaUnit)
              .HasColumnName("pjlnd_subareaunit");

                entity.Property(e => e.PjLandLnd)
              .HasColumnName("pjlnd_land");

                entity.Property(e => e.UtSlno)
              .HasColumnName("ut_slno");

                entity.Property(e => e.LoanAcc)
              .HasColumnName("ln_acc");

                entity.Property(e => e.LoanSub)
              .HasColumnName("ln_sub");

                entity.Property(e => e.PjLandSiteNo)
                 .HasMaxLength(50)
              .HasColumnName("pjlnd_site_no");

                entity.Property(e => e.PjLandAddress)
                 .HasMaxLength(200)
              .HasColumnName("pjlnd_address");

                entity.Property(e => e.PjLandDim)
                 .HasMaxLength(50)
              .HasColumnName("pjlnd_dim");

                entity.Property(e => e.PjLandLndDetails)
                .HasMaxLength(500)
              .HasColumnName("pjlnd_land_details");

                entity.Property(e => e.PjLandLocation)
                .HasMaxLength(500)
               .HasColumnName("pjlnd_location");

                entity.Property(e => e.PjLandLandMark)
               .HasMaxLength(200)
              .HasColumnName("pjlnd_landmark");

                entity.Property(e => e.PjLandConversationDet)
               .HasMaxLength(500)
              .HasColumnName("pjlnd_conversation_det");

                entity.Property(e => e.FinanceKsfc)
              .HasColumnName("finance_ksfc");

                entity.Property(e => e.Existingland)
              .HasColumnName("existing_land");

                entity.Property(e => e.PjLandNotes)
              .HasColumnName("pjlnd_notes");

                entity.Property(e => e.PjLandUpload)
                .HasMaxLength(300)
              .HasColumnName("pjlnd_upload");

                entity.Property(e => e.PjLandItemNo)
              .HasColumnName("pjlnd_itemno");

                entity.Property(e => e.IsActive)
               .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
               .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
             .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                 .HasMaxLength(50)
             .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
             .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
             .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
               .HasMaxLength(200)
              .HasColumnName("unique_id");
            });


            modelBuilder.Entity<TblIdmProjBldg>(entity =>
            {

                entity.HasKey(e => e.PjBdgRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_proj_bldg");

                entity.HasIndex(e => e.PjBdgOffc, "fk_tbl_idm_proj_bldg_offc_cdtab");
                entity.HasIndex(e => e.PjBdgUnit, "fk_tbl_idm_proj_land_tbl_app_loan_mast");
                entity.HasIndex(e => e.PjBdgDeprMethod, "fk_tbl_idm_proj_land_tbl_depr_method_mast");


                entity.Property(e => e.PjBdgRowId)
                .HasColumnName("pjbdg_rowid");

                entity.Property(e => e.UtCd)
                .HasColumnName("ut_cd");

                entity.Property(e => e.UtSlno)
                .HasColumnName("ut_slno");

                entity.Property(e => e.LoanAcc)
                .HasColumnName("ln_acc");

                entity.Property(e => e.LoanSub)
               .HasColumnName("ln_sub");

                entity.Property(e => e.PjBdgOffc)
              .HasColumnName("pjbdg_offc");

                entity.Property(e => e.PjBdgUnit)
              .HasColumnName("pjbdg_unit");

                entity.Property(e => e.PjBdgSno)
              .HasColumnName("pjbdg_sno");

                entity.Property(e => e.PjBdgItemNo)
              .HasColumnName("pjbdg_itm_no");

                entity.Property(e => e.PjBdgdets)
                .HasMaxLength(600)
              .HasColumnName("pjbdg_dets");

                entity.Property(e => e.PjBdgRoof)
                .HasMaxLength(40)
              .HasColumnName("pjbdg_roof");

                entity.Property(e => e.PjBdgPlnthO)
              .HasColumnName("pjbdg_plnth_o");

                entity.Property(e => e.PjBdgPlnthR)
             .HasColumnName("pjbdg_plnth_r");

                entity.Property(e => e.PjBdgUcostO)
             .HasColumnName("pjbdg_ucosto");

                entity.Property(e => e.PjBdgUcostR)
             .HasColumnName("pjbdg_ucostr");

                entity.Property(e => e.PjBdgTcostO)
             .HasColumnName("pjbdg_tcosto");

                entity.Property(e => e.PjBdgTcostR)
             .HasColumnName("pjbdg_tcostr");

                entity.Property(e => e.SubVentionBdg)
             .HasColumnName("subvention_bdg");

                entity.Property(e => e.ApbsTotCst)
            .HasColumnName("apbs_totcst");

                entity.Property(e => e.ExistingBdg)
             .HasColumnName("existing_bdg");

                entity.Property(e => e.Contingency)
             .HasColumnName("contingency");

                entity.Property(e => e.PjBdgNote)
             .HasColumnName("pjbdg_note");

                entity.Property(e => e.PjBdgSubvNote)
             .HasColumnName("pjbdg_subv_note");

                entity.Property(e => e.PjBdgUpload)
               .HasMaxLength(300)
             .HasColumnName("pjbdg_upload");

                entity.Property(e => e.IsActive)
             .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
             .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
             .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                 .HasMaxLength(50)
             .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
             .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
            .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                 .HasMaxLength(200)
             .HasColumnName("unique_id");

                entity.Property(e => e.PjBdgConsValue)
             .HasColumnName("pjbdg_cons_value");

                entity.Property(e => e.PjBdgDiffValue)
             .HasColumnName("pjbdg_diff_value");

                entity.Property(e => e.PjBdgDeprMethod)
             .HasColumnName("pjbdg_depr_method");

                entity.Property(e => e.PjBdgDeprValue)
            .HasColumnName("pjbdg_depr_value");

                entity.Property(e => e.PjBdgSubvCost)
             .HasColumnName("pjbdg_subv_cost");

            });

            modelBuilder.Entity<TblIdmProjPlmc>(entity =>
            {

                entity.HasKey(e => e.PjPlmcRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_proj_plmc");

                entity.HasIndex(e => e.PjPlmcOffc, "fk_tbl_idm_proj_plmc_offc_cdtab");
                entity.HasIndex(e => e.PjPlmcUnit, "fk_tbl_idm_proj_plmc_tbl_app_loan_mast");
              
                entity.Property(e => e.PjPlmcRowId)
            .HasColumnName("pjplmc_rowid");

                entity.Property(e => e.UtCd)
            .HasColumnName("ut_cd");

                entity.Property(e => e.UtSlno)
            .HasColumnName("ut_slno");

                entity.Property(e => e.LoanAcc)
            .HasColumnName("ln_acc");

                entity.Property(e => e.LoanSub)
            .HasColumnName("ln_sub");

                entity.Property(e => e.PjPlmcOffc)
             .HasColumnName("pjplmc_offc");

                entity.Property(e => e.PjPlmcUnit)
            .HasColumnName("pjplmc_unit");

                entity.Property(e => e.PjPlmcSno)
            .HasColumnName("pjplmc_sno");

                entity.Property(e => e.PjPlmcDets)
                .HasMaxLength(600)
           .HasColumnName("pjplmc_dets");

                entity.Property(e => e.PjPlmcSup)
                .HasMaxLength(100)
           .HasColumnName("pjplmc_sup");

                entity.Property(e => e.PjPlmcSupadr)
                .HasMaxLength(500)
            .HasColumnName("pjplmc_supadr");

                entity.Property(e => e.PjPlmcInvNo)
                .HasMaxLength(100)
            .HasColumnName("pjplmc_inv_no");

                entity.Property(e => e.PjPlmcInvDt)
             .HasColumnName("pjplmc_inv_dt");

                entity.Property(e => e.PjplmcCletStat)
              .HasColumnName("pjplmc_clet_stat");

                entity.Property(e => e.PjplmcReg)
             .HasColumnName("pjplmc_reg");

                entity.Property(e => e.PjplmcQty)
             .HasColumnName("pjplmc_qty");

                entity.Property(e => e.PjplmcStat)
             .HasColumnName("pjplmc_stat");

                entity.Property(e => e.PjplmcDelry)
             .HasColumnName("pjplmc_delry");

                entity.Property(e => e.PjplmcCst)
             .HasColumnName("pjplmc_cst");

                entity.Property(e => e.PjplmcTax)
             .HasColumnName("pjplmc_tax");

                entity.Property(e => e.PjplmcTotCst)
             .HasColumnName("pjplmc_totcst");

                entity.Property(e => e.PjplmcTotCstr)
               .HasColumnName("pjplmc_totcstr");

                entity.Property(e => e.PjplmcItemNo)
             .HasColumnName("pjplmc_itm_no");

                entity.Property(e => e.PjplmcNonSsi)
             .HasColumnName("pjplmc_non_ssi");

                entity.Property(e => e.PjplmcQtyr)
             .HasColumnName("pjplmc_qtyr");

                entity.Property(e => e.PjplmcContingency)
             .HasColumnName("pjplmc_contingency");

                entity.Property(e => e.PjplmcExisting)
             .HasColumnName("pjplmc_existing");

                entity.Property(e => e.SubventionPlmc)
             .HasColumnName("subvention_plmc");

                entity.Property(e => e.PjplmcSubvCost)
             .HasColumnName("pjplmc_subv_cost");

                entity.Property(e => e.PjplmcDeprmethod)
             .HasColumnName("pjplmc_depr_method");

                entity.Property(e => e.PjplmcDeprvalue)
             .HasColumnName("pjplmc_depr_value");

                entity.Property(e => e.PjplmcDirectProd)
             .HasColumnName("pjplmc_direct_prod");

                entity.Property(e => e.PjplmcSubvNotes)
            .HasColumnName("pjplmc_subv_notes");

                entity.Property(e => e.PjplmcUpload)
                  .HasMaxLength(300)
            .HasColumnName("pjplmc_upload");

                entity.Property(e => e.IsActive)
            .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                 .HasMaxLength(50)
            .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
            .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
             .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
             .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
               .HasMaxLength(200)
              .HasColumnName("unique_id");

                entity.Property(e => e.PjPlmcGst)
               .HasMaxLength(50)
             .HasColumnName("pjplmc_gst");
            });


            modelBuilder.Entity<TblIdmProjImpMc>(entity =>
            {

                entity.HasKey(e => e.PjImcRowId)
                   .HasName("PRIMARY");

                entity.ToTable("tbl_idm_proj_imp_mc");

                entity.HasIndex(e => e.PjImcOffc, "fk_tbl_idm_proj_imp_mc_offc_cdtab");
                entity.HasIndex(e => e.PjImcUnit, "fk_tbl_idm_proj_imp_mc_tbl_app_loan_mast");

                entity.Property(e => e.PjImcRowId)
              .HasColumnName("pjimc_rowid");

                entity.Property(e => e.UtCd)
              .HasColumnName("ut_cd");

                entity.Property(e => e.UtSlno)
             .HasColumnName("ut_slno");

                entity.Property(e => e.LoanAcc)
             .HasColumnName("ln_acc");

                entity.Property(e => e.LoanSub)
             .HasColumnName("ln_sub");

                entity.Property(e => e.PjImcOffc)
             .HasColumnName("pjimc_offc");

                entity.Property(e => e.PjImcUnit)
             .HasColumnName("pjimc_unit");

                entity.Property(e => e.PjImcSno)
             .HasColumnName("pjimc_sno");

                entity.Property(e => e.PjImcItemNo)
             .HasColumnName("pjimc_itm_no");

                entity.Property(e => e.PjImcEntry)
                .HasMaxLength(50)
             .HasColumnName("pjimc_entry");

                entity.Property(e => e.PjImcEntryI)
                .HasMaxLength(50)
             .HasColumnName("pjimc_entry_i");

                entity.Property(e => e.PjImcCrncy)
                .HasMaxLength(50)
             .HasColumnName("pjimc_crncy");

                entity.Property(e => e.PjImcExrd)
             .HasColumnName("pjimc_exrd");

                entity.Property(e => e.PjImcCif)
             .HasColumnName("pjimc_cif");

                entity.Property(e => e.PjImcCduty)
             .HasColumnName("pjimc_cduty");

                entity.Property(e => e.PjImcTotCost)
             .HasColumnName("pjimc_tot_cost");

                entity.Property(e => e.PjImcStat)
             .HasColumnName("pjimc_stat");

                entity.Property(e => e.PjImcDets)
                .HasMaxLength(600)
             .HasColumnName("pjimc_dets");

                entity.Property(e => e.PjImcSup)
                .HasMaxLength(100)
             .HasColumnName("pjimc_sup");

                entity.Property(e => e.PjImcQty)
              .HasColumnName("pjimc_qty");

                entity.Property(e => e.PjImcDelry)
              .HasColumnName("pjimc_delry");

                entity.Property(e => e.PjImcSupadr)
             .HasColumnName("pjimc_supadr");

                entity.Property(e => e.PjImcCpct)
             .HasColumnName("pjimc_cpct");

                entity.Property(e => e.PjImcCamt)
             .HasColumnName("pjimc_camt");

                entity.Property(e => e.PjImcNonssi)
             .HasColumnName("pjimc_non_ssi");

                entity.Property(e => e.PjImcRcif)
             .HasColumnName("pjimc_rcif");

                entity.Property(e => e.PjImcRcrncy)
             .HasColumnName("pjimc_rcrncy");

                entity.Property(e => e.PjImcRexch)
             .HasColumnName("pjimc_rexch");

                entity.Property(e => e.PjImcRcDuty)
             .HasColumnName("pjimc_rcduty");

                entity.Property(e => e.PjImcRtotCost)
             .HasColumnName("pjimc_rtot_cost");

                entity.Property(e => e.PjImcRcpct)
             .HasColumnName("pjimc_rcpct");

                entity.Property(e => e.PjImcRcamt)
             .HasColumnName("pjimc_rcamt");

                entity.Property(e => e.PjImcBasicCost)
             .HasColumnName("pjimc_basic_cost");

                entity.Property(e => e.PjImcSupRegd)
            .HasColumnName("pjimc_sup_regd");

                entity.Property(e => e.PjImcBoeno)
            .HasColumnName("pjimc_boeno");

                entity.Property(e => e.PjImcExisting)
            .HasColumnName("pjimc_existing");

                entity.Property(e => e.PjImcSubvention)
            .HasColumnName("pjimc_subvention");

                entity.Property(e => e.PjImcSubvCost)
            .HasColumnName("pjimc_subv_cost");

                entity.Property(e => e.PjImcDeprMethod)
            .HasColumnName("pjimc_depr_method");

                entity.Property(e => e.PjImcDeprValue)
            .HasColumnName("pjplmc_depr_value");

                entity.Property(e => e.PjImcDirectProd)
            .HasColumnName("pjimc_direct_prod");

                entity.Property(e => e.PjImcContingency)
            .HasColumnName("pjimc_contingency");

                entity.Property(e => e.PjImcSubvNotes)
            .HasColumnName("pjimc_subv_notes");

                entity.Property(e => e.PjImcNotes)
             .HasColumnName("pjimc_notes");

                entity.Property(e => e.PjImcUpload)
                .HasMaxLength(300)
             .HasColumnName("pjimc_upload");

                entity.Property(e => e.PjImcGst)
            .HasColumnName("pjimc_gst");

                entity.Property(e => e.IsActive)
            .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
            .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                 .HasMaxLength(50)
            .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                 .HasMaxLength(50)
            .HasColumnName("modified_by");

                entity.Property(e => e.UniqueId)
            .HasColumnName("unique_id");

                entity.Property(e => e.PjImcTotalBasicCost)
            .HasColumnName("pjimc_total_basic_cost");

                entity.Property(e => e.PjImcOtherExp)
             .HasColumnName("pjimc_oth_exp");

            });

            modelBuilder.Entity<TblIdmFurn>(entity =>
            {

                entity.HasKey(e => e.PjfRowId)
                  .HasName("PRIMARY");

                entity.ToTable("tbl_idm_furn");

                entity.HasIndex(e => e.PjfOffc, "fk_tbl_idm_furn_offc_cdtab");
                entity.HasIndex(e => e.PjfUnit, "fk_tbl_idm_furn_tbl_app_loan_mast");

                entity.Property(e => e.PjfRowId)
              .HasColumnName("pjf_rowid");

                entity.Property(e => e.UtCd)
             .HasColumnName("ut_cd");

                entity.Property(e => e.UtSlno)
             .HasColumnName("ut_slno");

                entity.Property(e => e.LoanAcc)
             .HasColumnName("ln_acc");

                entity.Property(e => e.PjfOffc)
             .HasColumnName("pjf_offc");

                entity.Property(e => e.PjfUnit)
             .HasColumnName("pjf_unit");

                entity.Property(e => e.PjfSno)
             .HasColumnName("pjf_sno");

                entity.Property(e => e.PjfItemNo)
             .HasColumnName("pjf_itm_no");

                entity.Property(e => e.PjfDets)
                .HasMaxLength(600)
             .HasColumnName("pjf_dets");

                entity.Property(e => e.PjfCst)
             .HasColumnName("pjf_cst");

                entity.Property(e => e.PjfQty)
             .HasColumnName("pjf_qty");

                entity.Property(e => e.PjfTax)
             .HasColumnName("pjf_tax");

                entity.Property(e => e.PjfTotcst)
             .HasColumnName("pjf_totcst");

                entity.Property(e => e.PjfCpct)
             .HasColumnName("pjf_cpct");

                entity.Property(e => e.PjfRcpct)
             .HasColumnName("pjf_rcpct");

                entity.Property(e => e.PjfContingency)
              .HasColumnName("pjf_contingency");

                entity.Property(e => e.PjfCamt)
             .HasColumnName("pjf_camt");

                entity.Property(e => e.PjfRcamt)
             .HasColumnName("pjf_rcamt");

                entity.Property(e => e.PjfSup)
                .HasMaxLength(100)
             .HasColumnName("pjf_sup");

                entity.Property(e => e.PjfSupadr)
                .HasMaxLength(500)
             .HasColumnName("pjf_supadr");

                entity.Property(e => e.PjfReg)
              .HasColumnName("pjf_reg");

                entity.Property(e => e.PjfInvNo)
                .HasMaxLength(100)
             .HasColumnName("pjf_inv_no");

                entity.Property(e => e.PjfInvDt)
             .HasColumnName("pjf_inv_dt");

                entity.Property(e => e.PjfCletStat)
             .HasColumnName("pjf_clet_stat");

                entity.Property(e => e.PjfStat)
             .HasColumnName("pjf_stat");

                entity.Property(e => e.PjfDelry)
             .HasColumnName("pjf_delry");

                entity.Property(e => e.PjfTotCstr)
             .HasColumnName("pjf_totcstr");

                entity.Property(e => e.PjfNonSsi)
             .HasColumnName("pjf_non_ssi");

                entity.Property(e => e.PjfAqrdStat)
             .HasColumnName("pjf_aqrd_stat");

                entity.Property(e => e.PjfExisting)
             .HasColumnName("pjf_existing");

                entity.Property(e => e.PjfNotes)
             .HasColumnName("pjf_notes");

                entity.Property(e => e.PjfUpload)
               .HasMaxLength(300)
              .HasColumnName("pjf_upload");

                entity.Property(e => e.IsActive)
             .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
             .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
             .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
             .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
             .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
             .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
            .HasColumnName("unique_id");

            });
            /// <summary>
            ///  Author: Swetha ; Module: Working Capital Inspection; Date:29/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDchgWorkingCapital>(entity =>
            {
                entity.HasKey(e => e.DcwcRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_wc");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_wctbl_app_loan_mast");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_wc_offc_cdtab");

                entity.Property(e => e.DcwcRowId)
                    .HasColumnName("dcwc_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");
                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");
                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DcwcNbank)
                .HasMaxLength(30)
                    .HasColumnName("dcwc_nbank");

                entity.Property(e => e.DcwcObank)
                   .HasMaxLength(30)
                    .HasColumnName("dcwc_obank");

                entity.Property(e => e.DcwcOnoc)
                    .HasMaxLength(30)
                    .HasColumnName("dcwc_onoc");

                entity.Property(e => e.DcwcOnocdt)
                    .HasColumnName("dcwc_onocdt");

                entity.Property(e => e.DcwcSandt)
                    .HasColumnName("dcwc_sandt");

                entity.Property(e => e.DcwcAmount)
                    .HasColumnName("dcwc_amount");

                entity.Property(e => e.DcwcRem)
                    .HasColumnName("dcwc_rem");

                entity.Property(e => e.DcwcMemdt)
                    .HasColumnName("dcwc_memdt");

                entity.Property(e => e.DcwcIno)
                   .HasColumnName("dcwc_ino");

                entity.Property(e => e.DcwcNbnkAdr1)
                  .HasColumnName("dcwc_nbnkadr1");

                entity.Property(e => e.DcwcNbnkAdr2)
                 .HasColumnName("dcwc_nbnkadr2");

                entity.Property(e => e.DcwcNbnkAdr3)
                 .HasColumnName("dcwc_nbnkadr3");


                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)

                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)

                    .HasColumnName("is_deleted");
            });

            /// <summary>
            ///  Author: Manoj ; Module: Land Inspection; Date:25/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmIrBldgMat>(entity =>
            {
                entity.HasKey(e => e.IrbmRowId)
                    .HasName("PRIMARY");
                entity.ToTable("tbl_idm_ir_bldgmat");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_land_tbl_app_loan_mast");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_land_offc_cdtab");

                entity.Property(e => e.IrbmRowId)
                    .HasColumnName("irbm_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                  .HasColumnName("offc_cd");

                entity.Property(e => e.IrbmIdt)
                  .HasColumnName("irbm_idt");

                entity.Property(e => e.IrbmRdt)
                .HasMaxLength(10)
                    .HasColumnName("irbm_rdt");

                entity.Property(e => e.IrbmItem)
                    .HasColumnName("irbm_item");

                entity.Property(e => e.IrbmMat)
                .HasMaxLength(100)
                    .HasColumnName("irbm_mat");

                entity.Property(e => e.IrbmQty)
                    .HasColumnName("irbm_qty");

                entity.Property(e => e.IrbmRate)
                    .HasColumnName("irbm_rate");

                entity.Property(e => e.IrbmNo)
                    .HasColumnName("irbm_no");

                entity.Property(e => e.IrbmQtyIn)
                .HasMaxLength(10)
                    .HasColumnName("irbm_qtyin");

                entity.Property(e => e.UomId)
                    .HasColumnName("uom_id");

                entity.Property(e => e.IrbmIno)
                   .HasColumnName("irbm_ino");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)
                   .HasDefaultValue(true)
                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.IrbmAmt)
                    .HasColumnName("irbm_amt");
            });
            /// <summary>
            ///  Author: Swetha ; Module: Import Machinery Isnpection ; Date:25/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDchgImportMachinery>(entity =>
            {
                entity.HasKey(e => e.DimcRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_imp_mc");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_imp_mc_tbl_app_loan_mast");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_imp_mc_offc_cdtab");

                entity.Property(e => e.DimcRowId)
                    .HasColumnName("dimc_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");
                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");
                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DimcItmNo)

                    .HasColumnName("dimc_itm_no");

                entity.Property(e => e.DimcEntry)

                   .HasColumnName("dimc_entry");

                entity.Property(e => e.DimcEntryI)

                  .HasColumnName("dimc_entry_i");

                entity.Property(e => e.DimcCrncy)

                  .HasColumnName("dimc_crncy");

                entity.Property(e => e.DimcExrd)

                  .HasColumnName("dimc_exrd");

                entity.Property(e => e.DimcCduty)

                  .HasColumnName("dimc_cduty");

                entity.Property(e => e.DimcTotCost)

                  .HasColumnName("dimc_tot_cost");


                entity.Property(e => e.DimcStat)

                 .HasColumnName("dimc_stat");

                entity.Property(e => e.DimcDets)

                 .HasColumnName("dimc_dets");

                entity.Property(e => e.DimcSup)
                 .HasColumnName("dimc_sup");

                entity.Property(e => e.DimcQty)
                 .HasColumnName("dimc_qty");


                entity.Property(e => e.DimcDelry)
                 .HasColumnName("dimc_delry");

                entity.Property(e => e.DimcSupAdr1)
                 .HasColumnName("dimc_supadr1");

                entity.Property(e => e.DimcSupAdr2)
                .HasColumnName("dimc_supadr2");

                entity.Property(e => e.DimcSupAdr3)
                .HasColumnName("dimc_supadr3");

                entity.Property(e => e.DimcCpct)
              .HasColumnName("dimc_cpct");


                entity.Property(e => e.DimcCamt)
              .HasColumnName("dimc_camt");

                entity.Property(e => e.DimcAqrdStat)
              .HasColumnName("dimc_aqrd_stat");

                entity.Property(e => e.DimcApau)
              .HasColumnName("dimc_apau");

                entity.Property(e => e.DimcApDate)
              .HasColumnName("dimc_apdate");


                entity.Property(e => e.DimcCletStat)
              .HasColumnName("dimc_clet_stat");

                entity.Property(e => e.DimcActualCost)
              .HasColumnName("dimc_actual_cost");

                entity.Property(e => e.DimcAqrdIndicator)
              .HasColumnName("dimc_aqrd_indicator");

                entity.Property(e => e.DimcBankAdivce)
              .HasColumnName("dimc_bank_advice");

                entity.Property(e => e.DimcCif)
              .HasColumnName("dimc_cif");

                entity.Property(e => e.DimcBankAdvDate)
            .HasColumnName("dimc_bank_advdate");

                entity.Property(e => e.DimcMacDocuments)
              .HasColumnName("dimc_mac_documents");

                entity.Property(e => e.DimcStatChgDate)
            .HasColumnName("dimc_stat_chgdate");

                entity.Property(e => e.Dimcsec)
            .HasColumnName("dimc_sec");

                entity.Property(e => e.DimcIno)
            .HasColumnName("dimc_ino");

                entity.Property(e => e.DimcsecRel)
                     .HasColumnName("dimc_sec_rel");

                entity.Property(e => e.DimcsecElig)
                .HasColumnName("dimc_sec_elig");


                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)
                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.DimcStatDesc)
                 .HasColumnName("dimc_stat_desc");

                entity.Property(e => e.DimcGST)
                 .HasColumnName("dimc_gst");

                entity.Property(e => e.DimcOthExp)
                 .HasColumnName("dimc_othexp");

                entity.Property(e => e.DimcTotalCostMem)
                 .HasColumnName("dimc_totcost_mem");

                entity.Property(e => e.DimcBasicAmt)
                .HasColumnName("dimc_basicamt");

                entity.Property(e => e.DimcCurBasicAmt)
                .HasColumnName("dimc_cur_basicamt");

            });


            modelBuilder.Entity<TblIdmDChgFurn>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_furn");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_furn_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_furn_offc_cdtab");

                entity.Property(e => e.Id)
                    .HasColumnName("dfurn_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.FurnDetails)
                 .HasMaxLength(200)
                    .HasColumnName("dfurn_dets");

                entity.Property(e => e.FurnSupp)
                 .HasMaxLength(100)
                    .HasColumnName("dfurn_sup");

                entity.Property(e => e.FurnSuppAdd1)
                 .HasMaxLength(100)
                   .HasColumnName("dfurn_supadr1");

                entity.Property(e => e.FurnSuppAdd2)
                 .HasMaxLength(100)
                    .HasColumnName("dfurn_supadr2");

                entity.Property(e => e.FurnSuppAdd3)
                 .HasMaxLength(100)
                    .HasColumnName("dfurn_supadr3");

                entity.Property(e => e.FurnInvoiceNo)
                   .HasColumnName("dfurn_inv_no");

                entity.Property(e => e.FurnInvoiceDate)
                   .HasColumnName("dfurn_inv_dt");

                entity.Property(e => e.FurnCletStat)
                   .HasColumnName("dfurn_clet_stat");

                entity.Property(e => e.FurnReg)
                   .HasColumnName("dfurn_reg");

                entity.Property(e => e.FurnQty)
                  .HasColumnName("dfurn_qty");

                entity.Property(e => e.Stat)
                  .HasColumnName("stat");

                entity.Property(e => e.FurnDeleiverInWeek)
                 .HasColumnName("dfurn_delry");

                entity.Property(e => e.FurnCost)
                 .HasColumnName("dfurn_cst");

                entity.Property(e => e.FurnTax)
                 .HasColumnName("dfurn_tax");

                entity.Property(e => e.FurnTotalCost)
                 .HasColumnName("dfurn_totcst");

                entity.Property(e => e.FurnItemNo)
                 .HasColumnName("dfurn_itm_no");

                entity.Property(e => e.FurnAqrdStat)
                 .HasColumnName("dfurn_aqrd_stat");

                entity.Property(e => e.FurnRequDate)
                 .HasColumnName("dfurn_rqdt");

                entity.Property(e => e.FurnApdt)
                 .HasColumnName("dfurn_apdt");

                entity.Property(e => e.FurnApau)
                 .HasColumnName("dfurn_apau");

                entity.Property(e => e.FurnNonSsi)
                 .HasColumnName("dfurn_non_ssi");

                entity.Property(e => e.FurnActualCost)
                 .HasColumnName("dfurn_actual_cost");

                entity.Property(e => e.FurnAqrdIndicator)
                 .HasColumnName("dfurn_aqrd_indicator");

                entity.Property(e => e.FurnStatChangeDate)
                 .HasColumnName("dfurn_stat_chgdate");


                entity.Property(e => e.FurnBankAdvice)
                 .HasMaxLength(100)
               .HasColumnName("dfurn_bank_advice");

                entity.Property(e => e.FurnBankAdvDate)
                 .HasMaxLength(100)
               .HasColumnName("dfurn_bank_advdate");

                entity.Property(e => e.FurnBankName)
                 .HasMaxLength(100)
               .HasColumnName("dfurn_bank_name");

                entity.Property(e => e.FurnSat)
               .HasColumnName("dfurn_stat");

                entity.Property(e => e.FurnSec)
               .HasColumnName("dfurn_sec");

                entity.Property(e => e.FurnIno)
               .HasColumnName("dfurn_ino");


                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueID)
                 .HasMaxLength(150)
               .HasColumnName("unique_id");

                entity.Property(e => e.DfurnSecRel)
               .HasColumnName("dfurn_sec_rel");

            });

            modelBuilder.Entity<TblStateZone>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_state_zone_cdtab");

                entity.Property(e => e.Id)
                    .HasColumnName("state_zone_cd");

                entity.Property(e => e.StateZoneDesc)
                .HasMaxLength(200)
                    .HasColumnName("state_zone_desc");

                entity.Property(e => e.CreateBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.IsActive)

                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)

                    .HasColumnName("is_deleted");
            });

            /// <summary>
            ///  Author: Manoj ; Module: Indigenous Machinery Inspection; Date:25/08/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDchgIndigenousMachineryDet>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_plmc");

                entity.HasIndex(e => e.LoanAcc, "loan_acc_idx");
                entity.HasIndex(e => e.OffcCd, "offc_cd_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("dcplmc_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.RequestDate)
                    .HasColumnName("dcplmc_rqdt");

                entity.Property(e => e.ApproveDate)
                    .HasColumnName("dcplmc_apdt");

                entity.Property(e => e.ApproveAU)
                    .HasColumnName("dcplmc_apau");

                entity.Property(e => e.ItemNo)
                    .HasColumnName("dcplmc_itmno");

                entity.Property(e => e.ItemDetails)
                .HasMaxLength(600)
                    .HasColumnName("dcplmc_dets");

                entity.Property(e => e.SupplierName)
                .HasMaxLength(80)
                    .HasColumnName("dcplmc_sup");

                entity.Property(e => e.InvoiceNo)
                .HasMaxLength(20)
                    .HasColumnName("dcplmc_inv_no");

                entity.Property(e => e.InvoiceDate)
                    .HasColumnName("dcplmc_inv_dt");

                entity.Property(e => e.RegisteredState)
                    .HasColumnName("dcplmc_reg");

                entity.Property(e => e.Quantity)
                    .HasColumnName("dcplmc_qty");

                entity.Property(e => e.MachineryStatus)
                    .HasColumnName("dcplmc_stat");

                entity.Property(e => e.Delivery)
                   .HasColumnName("dcplmc_delry");

                entity.Property(e => e.Cost)
                    .HasColumnName("dcplmc_cst");

                entity.Property(e => e.Tax)
                    .HasColumnName("dcplmc_tax");

                entity.Property(e => e.TotalCost)
                    .HasColumnName("dcplmc_tcost");

                entity.Property(e => e.CommunicationDate)
                    .HasColumnName("dcplmc_comdt");

                entity.Property(e => e.RequestNo)
                    .HasColumnName("dcplmc_rqno");

                entity.Property(e => e.SupplierAddress1)
                .HasMaxLength(30)
                    .HasColumnName("dcplmc_supadr1");

                entity.Property(e => e.SupplierAddress2)
                .HasMaxLength(30)
                   .HasColumnName("dcplmc_supadr2");

                entity.Property(e => e.SupplierAddress3)
                .HasMaxLength(30)
                    .HasColumnName("dcplmc_supadr3");

                entity.Property(e => e.Status)
                    .HasColumnName("dcplmc_clet_stat");

                entity.Property(e => e.AquiredStatus)
                    .HasColumnName("dcplmc_aqrd_stat");

                entity.Property(e => e.ActualCost)
                    .HasColumnName("dcplmc_actual_cost");

                entity.Property(e => e.BankAdvice)
                .HasMaxLength(3)
                    .HasColumnName("dcplmc_bank_advice");

                entity.Property(e => e.BankName)
                .HasMaxLength(20)
                    .HasColumnName("dcplmc_bank_name");

                entity.Property(e => e.BankAdviceDate)
                   .HasColumnName("dcplmc_bank_advdate");

                entity.Property(e => e.AquiredIndicator)
                    .HasColumnName("dcplmc_aqrd_indicator");

                entity.Property(e => e.StatusChangedDate)
                    .HasColumnName("dcplmc_stat_chgdate");

                entity.Property(e => e.CletValidity)
                .HasMaxLength(1)
                    .HasColumnName("dcplmc_clet_validity");

                entity.Property(e => e.SecurityCreated)
                   .HasColumnName("dcplmc_sec");

                entity.Property(e => e.Ino)
                    .HasColumnName("dcplmc_ino");

                entity.Property(e => e.SecurityRelease)
                    .HasColumnName("dcplmc_sec_rel");

                entity.Property(e => e.SecurityEligibility)
                    .HasColumnName("dcplmc_sec_elig");

                entity.Property(e => e.BasicCost)
                    .HasColumnName("dcplmc_basic_cost");

                entity.Property(e => e.IsActive)
                   .HasDefaultValue(true)
                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");
            });

            // Dev
            modelBuilder.Entity<TblAcTypeCdtab>(entity =>
            {
                entity.HasKey(e => e.AcTypeCd)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_actype_cdtab");

                entity.Property(e => e.AcTypeCd)
                    .HasColumnName("actype_cd");

                entity.Property(e => e.AcTypeDets)
                    .HasMaxLength(50)
                    .HasColumnName("actype_dets");

                entity.Property(e => e.AcTypeDisSeq)
                    .HasColumnName("actype_dis_seq");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");
            });

            /// <summary>
            ///  Author: Akhila ; Module:Project Cost Details : Date:05/09/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDchgProjectCost>(entity =>
            {
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_pcost");

                entity.HasIndex(e => e.LoanAcc, "loan_acc_idx");
                entity.HasIndex(e => e.OffcCd, "offc_cd_idx");

                entity.Property(e => e.Id)
                    .HasColumnName("dcpcst_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DcpcRequestDate)
                    .HasColumnName("dcpcst_rqdt");

                entity.Property(e => e.DcpcApprovedate)
                   .HasColumnName("dcpcst_apdt");

                entity.Property(e => e.DcpcstApproveAU)
                   .HasColumnName("dcpcst_apau");

                entity.Property(e => e.DcpcstCode)
                   .HasColumnName("dcpcst_cd");

                entity.Property(e => e.DcpcAmount)
                   .HasColumnName("dcpcst_amt");

                entity.Property(e => e.DcpcCommunicationDate)
                   .HasColumnName("dcpcst_comdt");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(200)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)
                   .HasDefaultValue(true)
                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");
            });


            /// <summary>
            ///  Author: Manoj ; Module: Letter Of Credit Details ; Date:05/09/2022
            /// </summary>

            modelBuilder.Entity<TblIdmDsbLetterOfCredit>(entity =>
            {
                entity.HasKey(e => e.DcLocRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dsb_lt_crdt");
                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_imp_mc_tbl_app_loan_mast");
                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_imp_mc_offc_cdtab");

                entity.Property(e => e.DcLocRowId)
                    .HasColumnName("dcloc_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");
                entity.Property(e => e.LoanSub)
                   .HasColumnName("loan_sub");
                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DlCrdtItmNo)

                    .HasColumnName("dlcrdt_itm_no");

                entity.Property(e => e.DlCrdtItmDets)
                .HasMaxLength(100)
                   .HasColumnName("dlcrdt_itm_dets");

                entity.Property(e => e.DlCrdtCrltNo)
                .HasMaxLength(10)
                  .HasColumnName("dlcrdt_crlt_no");

                entity.Property(e => e.DlCrdtDt)

                  .HasColumnName("dlcrdt_dt");

                entity.Property(e => e.DlCrdtAmt)

                  .HasColumnName("dlcrdt_amt");

                entity.Property(e => e.DlCrdtCif)

                  .HasColumnName("dlcrdt_cif");

                entity.Property(e => e.DlCrdtBank)

                  .HasColumnName("dlcrdt_bank");


                entity.Property(e => e.DlCrdtBnkadr1)
                .HasMaxLength(30)
                 .HasColumnName("dlcrdt_bnkadr1");

                entity.Property(e => e.DlCrdtBnkadr2)
                .HasMaxLength(30)
                 .HasColumnName("dlcrdt_bnkadr2");

                entity.Property(e => e.DlCrdtBnkadr3)
                .HasMaxLength(30)
                 .HasColumnName("dlcrdt_bnkadr3");

                entity.Property(e => e.DlCrdtRqdt)
                 .HasColumnName("dlcrdt_rqdt");

                entity.Property(e => e.DlCrdtOpenDt)
                 .HasColumnName("dlcrdt_open_dt");

                entity.Property(e => e.DlCrdtVdty)
                 .HasColumnName("dlcrdt_vdty");

                entity.Property(e => e.DlCrdtHondt)
                .HasColumnName("dlcrdt_hondt");

                entity.Property(e => e.DlCrdtAu)
                .HasColumnName("dlcrdt_au");

                entity.Property(e => e.DlCrdtCletStat)
              .HasColumnName("dlcrdt_clet_stat");


                entity.Property(e => e.DlCrdtMrgMny)
              .HasColumnName("dlcrdt_mrg_mny");

                entity.Property(e => e.DlCrdtSup)
              .HasColumnName("dlcrdt_sup");

                entity.Property(e => e.DlCrdtSupAddr)
              .HasColumnName("dlcrdt_sup_addr");

                entity.Property(e => e.DlCrdtAqrdStat)
              .HasColumnName("dlcrdt_aqrd_stat");

                entity.Property(e => e.DlCrdtTotalAmt)
              .HasColumnName("dlcrdt_total_amt");

                entity.Property(e => e.DlCrdtBankIfsc)
              .HasColumnName("dlcrdt_bank_ifsc");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsActive)

                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)

                    .HasColumnName("is_deleted");
            });

            modelBuilder.Entity<TblIdmDchgMeansOfFinance>(entity =>
            {
                entity.HasKey(e => e.DcmfRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dchg_mf");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_dchg_mf_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dchg_mf_offc_cdtab");

                entity.Property(e => e.DcmfRowId)
                    .HasColumnName("dcmf_rowid");

                entity.Property(e => e.LoanAcc)
                  .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                .HasColumnName("offc_cd");

                entity.Property(e => e.DcmfRqdt)
                    .HasColumnName("dcmf_rqdt");

                entity.Property(e => e.DcmfApdt)
                .HasColumnName("dcmf_apdt");

                entity.Property(e => e.DcmfApau)
                    .HasColumnName("dcmf_apau");

                entity.Property(e => e.DcmfCd)
                    .HasColumnName("dcmf_cd");

                entity.Property(e => e.DcmfAmt)
                .HasColumnName("dcmf_amt");

                entity.Property(e => e.DcmfMfType)
                    .HasColumnName("dcmf_mf_type");

                entity.Property(e => e.CreatedBy)
                .HasMaxLength(50)
                   .HasColumnName("created_by");

                entity.Property(e => e.CreatedDate)
                   .HasColumnName("created_date");

                entity.Property(e => e.ModifiedBy)
                .HasMaxLength(50)
                   .HasColumnName("modified_by");

                entity.Property(e => e.UniqueId)
                                .HasMaxLength(150)
                    .HasColumnName("unique_id");

                entity.Property(e => e.ModifiedDate)
                   .HasColumnName("modified_date");

                entity.Property(e => e.IsActive)

                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)

                    .HasColumnName("is_deleted");
            });
            modelBuilder.Entity<Tblfm8fm13CDTab>(entity =>
            {
                entity.HasKey(e => e.FormTypeCD)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_fm8_fm13_cdtab");

                entity.Property(e => e.FormTypeCD)
                    .HasColumnName("form_type_cd");

                entity.Property(e => e.FormType)
                    .HasColumnName("form_type");



                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });


            /// <summary>
            ///  Author:Gowtham S ; Date:26/08/2022
            /// </summary>
            modelBuilder.Entity<IdmUnitProducts>(entity =>
            {
                entity.HasKey(e => e.IdmUtproductRowid)
                    .HasName("PRIMARY");

                entity.ToTable("idm_unit_products");



                entity.HasIndex(e => e.ProdId, "fk_tbl_idm_unit_products_tbl_ind_cdtab");

                entity.HasIndex(e => e.ProdId, "fk_tbl_idm_unit_products_tbl_prod_cdtab");

                entity.HasIndex(e => e.OffcCd, "tbl_idm_unit_products_ibfk_2");

                entity.HasIndex(e => e.LoanAcc, "tbl_idm_unit_products_idfk_7");

                entity.Property(e => e.IdmUtproductRowid)
                    .HasColumnName("idm_utproduct_rowid");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");



                entity.Property(e => e.ProdCd)
                    .HasColumnName("prod_cd");

                entity.Property(e => e.UniqueId)
                   .HasColumnName("unique_id");

                entity.Property(e => e.ProdId)
                    .HasColumnName("prod_id");

                entity.Property(e => e.IndId)
                   .HasColumnName("ind_id");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.CreatedBy)
                                   .HasMaxLength(50)
                                   .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");


                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");


                entity.HasOne(e => e.TblProdCdtabs)
                     .WithOne(b => b.IdmUnitProducts)
                     .HasForeignKey<IdmUnitProducts>(b => b.ProdId);

                entity.HasOne(e => e.ProdIndNavigation)
                   .WithOne(b => b.IdmUnitProducts)
                   .HasForeignKey<IdmUnitProducts>(b => b.ProdId);
            });

            // MJ
            modelBuilder.Entity<TblIdmUnitBank>(entity =>
            {
                entity.HasKey(e => e.IdmUtBankRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_unit_bank");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_unit_bank_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_unit_bank_offc_cdtab");

                entity.HasIndex(e => e.UtCd, "fk_tbl_idm_unit_bank_tbl_unit_mast");

                entity.HasIndex(e => e.UtIfsc, "fk_tbl_idm_unit_bank_tbl_ifsc_master");

                entity.Property(e => e.IdmUtBankRowId)
                  .HasColumnName("idm_utbank_rowid");

                entity.Property(e => e.LoanAcc)
                  .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UtCd)
                  .HasColumnName("ut_cd");

                entity.Property(e => e.BankIfscId)
                 .HasColumnName("bank_ifsc_id");

                entity.Property(e => e.UtIfsc)
                    .HasColumnName("ut_ifsc");

                entity.Property(e => e.UtBankPincode)
                    .HasColumnName("ut_bank_pincode");

                entity.Property(e => e.UtBank)
                    .HasColumnName("ut_bank");

                entity.Property(e => e.UtBankBranch)
                  .HasColumnName("ut_bank_branch");

                entity.Property(e => e.UtBankAddress)
                    .HasColumnName("ut_bank_address");

                entity.Property(e => e.UtBankArea)
                    .HasColumnName("ut_bank_area");



                entity.Property(e => e.UtBankCity)
                    .HasColumnName("ut_bank_city");


                entity.Property(e => e.UtBankPhone)
                    .HasColumnName("ut_bank_phone");


                entity.Property(e => e.UtBankPrimary)
                    .HasColumnName("ut_bank_primary");


                entity.Property(e => e.UtBankState)
                    .HasColumnName("ut_bank_state");


                entity.Property(e => e.UtBankDistrict)
                    .HasColumnName("ut_bank_district");


                entity.Property(e => e.UtBankTaluka)
                    .HasColumnName("ut_bank_taluka");

                entity.Property(e => e.UtBankAccno)
                    .HasColumnName("ut_bank_accno");


                entity.Property(e => e.UtBankHolderName)
                    .HasColumnName("ut_bank_holdername");


                entity.Property(e => e.UtAccType)
                    .HasColumnName("ut_acc_type");




                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");


                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });

            /// <summary>
            ///  Author:Gowtham S ; Date:26/08/2022
            /// </summary>
            modelBuilder.Entity<IdmPromAssetDet>(entity =>
            {
                entity.HasKey(e => e.IdmPromassetId)
                    .HasName("PRIMARY");

                entity.ToTable("idm_prom_asset_det");

                entity.HasIndex(e => e.AssetCatCD, "fk_idm_prom_asset_det_tbl_assetcat_cdtab");

                entity.HasIndex(e => e.AssetTypeCD, "idm_prom_asset_det_tbl_assettype_cdtab");

                entity.HasIndex(e => e.PromoterCode, "idm_prom_asset_det_tbl_prom_cdtab");

                entity.HasIndex(e => e.OffcCd, "fk_idm_prom_asset_det_offc_cdtab");



                entity.Property(e => e.IdmPromassetId)
                    .HasColumnName("idm_promasset_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");



                entity.Property(e => e.UtCd)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.UniqueId)
                   .HasColumnName("unique_id");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.AssetCatCD)
                   .HasColumnName("assetcat_cd");

                entity.Property(e => e.AssetTypeCD)
                 .HasColumnName("assettype_cd");

                entity.Property(e => e.LandType)
                 .HasColumnName("land_type");

                entity.Property(e => e.IdmAssetSiteno)
                 .HasColumnName("idm_asset_siteno");

                entity.Property(e => e.IdmAssetaddr)
                 .HasColumnName("idm_asset_addr");

                entity.Property(e => e.IdmAssetDim)
                .HasColumnName("idm_asset_dim");


                entity.Property(e => e.IdmAssetArea)
                .HasColumnName("idm_asset_area");

                entity.Property(e => e.IdmAssetDesc)
                .HasColumnName("idm_asset_desc");

                entity.Property(e => e.IdmAssetValue)
               .HasColumnName("idm_asset_value");

                entity.Property(e => e.IsActive).HasColumnName("is_active");

                entity.Property(e => e.IsDeleted).HasColumnName("is_deleted");

                entity.Property(e => e.CreatedDate).HasColumnName("created_date");

                entity.Property(e => e.CreatedBy)
                                   .HasMaxLength(50)
                                   .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                        .HasMaxLength(50)
                        .HasColumnName("modified_by");


                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

                entity.HasOne(e => e.TblAssettypeCdtabs)
                    .WithOne(b => b.IdmPromAssetDet)
                    .HasForeignKey<IdmPromAssetDet>(b => b.AssetTypeCD);

                entity.HasOne(e => e.AssetcatCdNavigation)
                   .WithOne(b => b.IdmPromAssetDet)
                   .HasForeignKey<IdmPromAssetDet>(b => b.AssetCatCD);

                entity.HasOne(e => e.TblPromCdtab)
                  .WithOne(b => b.IdmPromAssetDet)
                  .HasForeignKey<IdmPromAssetDet>(b => b.PromoterCode);



            });

            modelBuilder.Entity<TblPromterLiabDet>(entity =>
            {
                entity.HasKey(e => e.PromLiabId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_prom_liab_det");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_prom_liab_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_prom_liab_det_offc_cdtab");

                entity.HasIndex(e => e.UTCD, "fk_tbl_idm_prom_liab_det_tbl_unit_mast");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_idm_prom_liab_det_tbl_prom_cdtab");

                entity.Property(e => e.PromLiabId)
                    .HasColumnName("idm_promliab_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UTCD)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.LiabDesc)
                .HasMaxLength(100)
                   .HasColumnName("idm_liab_desc");

                entity.Property(e => e.LiabVal)
                    .HasColumnName("idm_liab_value");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueID)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblIdmPromoterNetWork>(entity =>
            {
                entity.HasKey(e => e.PromNetWrId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_prom_nw_det");

                entity.HasIndex(e => e.LoanAcc, "fk_tbl_idm_prom_nw_det_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_prom_nw_det_offc_cdtab");

                entity.HasIndex(e => e.UTCD, "fk_tbl_idm_prom_nw_det_tbl_unit_mast");

                entity.HasIndex(e => e.PromoterCode, "fk_tbl_idm_prom_nw_det_tbl_prom_cdtab");

                entity.Property(e => e.PromNetWrId)
                    .HasColumnName("idm_prom_nw_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.UTCD)
                    .HasColumnName("ut_cd");

                entity.Property(e => e.PromoterCode)
                    .HasColumnName("promoter_code");

                entity.Property(e => e.Idminmov)
                   .HasColumnName("idm_immov");

                entity.Property(e => e.IdmMov)
                   .HasColumnName("idm_mov");

                entity.Property(e => e.IdmLiab)
                    .HasColumnName("idm_liab");

                entity.Property(e => e.IdmNetworth)
                    .HasColumnName("idm_nw");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                        .HasColumnName("modified_date");

                entity.Property(e => e.UniqueID)
                       .HasColumnName("unique_id");

            });

            modelBuilder.Entity<TblLaReceiptDet>(entity =>
            {
                /// <summary>
                ///  Author: Gagana K; Module:  Generate Receipt; Date:14/09/2022
                /// </summary>
                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_la_receipt");

                entity.HasIndex(e => e.TransTypeId, "tbl_la_receipt_ibfk_1");

                entity.HasIndex(e => e.LoanNo, "tbl_la_receipt_ibfk_2");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.TransTypeId)
                       .HasColumnName("trans_type_id");

                entity.Property(e => e.LoanNo)
                      .HasColumnName("loan_no");

                entity.Property(e => e.ReceiptRefNo)
                        .HasColumnName("receipt_ref_no");

                entity.Property(e => e.ReceiptStatus)
                        .HasColumnName("receipt_status");

                entity.Property(e => e.DateOfGeneration)
                        .HasColumnName("date_of_generation");

                entity.Property(e => e.AmountDue)
                        .HasColumnName("amount_due");

                entity.Property(e => e.DueDatePayment)
                        .HasColumnName("due_date_payment");

                entity.Property(e => e.Remarks)
                       .HasColumnName("remarks");

                entity.Property(e => e.IsActive)
                        .HasDefaultValue(true)
                        .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                        .HasMaxLength(50)
                        .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                        .HasMaxLength(50)
                        .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                        .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                        .HasColumnName("modified_date");

                entity.HasOne(e => e.CodeTable)
                .WithOne(b => b.TblLaReceiptDet)
              .HasForeignKey<TblLaReceiptDet>(b => b.TransTypeId);
            });

            modelBuilder.Entity<TblLaReceiptPaymentDet>(entity =>
            {

                entity.HasKey(e => e.ReceiptPaymentId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_la_receipt_payment");

                entity.HasIndex(e => e.PaymentId, "tbl_la_receipt_payment_ibfk_1");

                entity.HasIndex(e => e.ReceiptId, "tbl_la_receipt_payment_ibfk_2");

                entity.Property(e => e.ReceiptPaymentId)
                    .HasColumnName("id");

                entity.Property(e => e.PaymentId)
                    .HasColumnName("payment_id");

                entity.Property(e => e.ReceiptId)
                    .HasColumnName("receipt_id");

                entity.Property(e => e.ReceiptPaymentStatus)
                     .HasColumnName("receipt_payment_status");

                entity.Property(e => e.DateofInitiation)
                     .HasColumnName("date_of_initiation");

                entity.Property(e => e.PaymentAmt)
                        .HasColumnName("payment_amt");

                entity.Property(e => e.TotalAmt)
                        .HasColumnName("total_amt");

                entity.Property(e => e.ActualAmt)
                        .HasColumnName("actual_amt");

                entity.Property(e => e.BalanceAmt)
                        .HasColumnName("balance_amt");

                entity.Property(e => e.IsActive)
                        .HasDefaultValue(true)
                        .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                        .HasMaxLength(50)
                        .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                        .HasMaxLength(50)
                        .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                        .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                        .HasColumnName("modified_date");

                entity.Property(e => e.UniqueID)
                       .HasColumnName("unique_id");

                entity.HasOne(e => e.TblLaReceiptDet)
                .WithOne(b => b.TblLaReceiptPaymentDet)
              .HasForeignKey<TblLaReceiptPaymentDet>(b => b.ReceiptId);


                entity.HasOne(d => d.TblLaPaymentDet)
                .WithMany(p => p.TblLaReceiptPaymentDet)
                .HasForeignKey(d => d.PaymentId);



            });

            modelBuilder.Entity<TblLaPaymentDet>(entity =>
            {

                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_la_payment");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.LoanNo)
                   .HasColumnName("loan_no");

                entity.Property(e => e.PromoterId)
                   .HasColumnName("promoter_id");

                entity.Property(e => e.PaymentRefNo)
                  .HasColumnName("payment_ref_no");

                entity.Property(e => e.ActualAmt)
                   .HasColumnName("actual_amt");

                entity.Property(e => e.DateOfInitiation)
                   .HasColumnName("date_of_initiation");

                entity.Property(e => e.PromoterName)
                        .HasColumnName("promoter_name");

                entity.Property(e => e.ChequeNo)
                        .HasColumnName("cheque_no");

                entity.Property(e => e.ChequeDate)
                        .HasColumnName("cheque_date");

                entity.Property(e => e.IfscCode)
                        .HasColumnName("ifsc_code");

                entity.Property(e => e.BranchCode)
                        .HasColumnName("branch_code");

                entity.Property(e => e.DateOfChequeRealization)
                       .HasColumnName("date_of_cheque_realization");

                entity.Property(e => e.UtrNo)
                .HasColumnName("utr_no");

                entity.Property(e => e.PaidDate)
                      .HasColumnName("paid_date");

                entity.Property(e => e.PaymentMode)
                   .HasColumnName("payment_mode");

                entity.Property(e => e.PaymentStatus)
                   .HasColumnName("payment_status");

                entity.Property(e => e.IsActive)
                        .HasDefaultValue(true)
                        .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                        .HasDefaultValue(false)
                        .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                        .HasMaxLength(50)
                        .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                        .HasMaxLength(50)
                        .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                        .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                        .HasColumnName("modified_date");
            });
            /// <summary>
            ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
            /// </summary>
            modelBuilder.Entity<TblIdmDhcgAllc>(entity =>
            {
                entity.HasKey(e => e.DcalcId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dhcg_allc");

                entity.HasIndex(e => e.LoanAcc, "FK_tbl_idm_dhcg_allc_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "fk_tbl_idm_dhcg_allc_offc_cdtab");

                entity.HasIndex(e => e.DcalcCd, "FK_tbl_idm_dhcg_tbl_allc_cdtab");

                entity.Property(e => e.DcalcId)
                    .HasColumnName("dcalc_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DcalcCd)
                    .HasColumnName("dcalc_cd");

                entity.Property(e => e.DcalcAmt)
                    .HasColumnName("dcalc_amt");

                entity.Property(e => e.DcalcRqdt)
                   .HasColumnName("dcalc_rqdt");

                entity.Property(e => e.DcalcApdt)
                   .HasColumnName("dcalc_apdt");

                entity.Property(e => e.DcalcComdt)
                    .HasColumnName("dcalc_comdt");

                entity.Property(e => e.DcalcApau)
                    .HasColumnName("dcalc_apau");

                entity.Property(e => e.DcalcAmtRevised)
                    .HasColumnName("dcalc_amt_revised");

                entity.Property(e => e.DcalcDetails)
                    .HasColumnName("dcalc_details");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");
            });

            /// <summary>
            ///  Author: Gagana K; Module:Loan Alloaction; Date:28/09/2022
            /// </summary>
            modelBuilder.Entity<TblAllcCdTab>(entity =>
            {
                entity.HasKey(e => e.AllcId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_allc_cdtab");

                entity.Property(e => e.AllcId)
                    .HasColumnName("allc_id");

                entity.Property(e => e.AllcCd)
                    .HasColumnName("allc_cd");

                entity.Property(e => e.AllcDets)
                    .HasColumnName("allc_dets");

                entity.Property(e => e.AllcFlg)
                   .HasColumnName("allc_flg");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");
            });

            // UC06

            modelBuilder.Entity<TblLandTypeMast>(entity =>
            {
                entity.HasKey(e => e.LandTypeId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_landtype_mast");

                entity.Property(e => e.LandTypeId)
                    .HasColumnName("landtype_id");

                entity.Property(e => e.LandTypeCd)
                    .HasColumnName("landtype_cd");

                entity.Property(e => e.LandTypeDesc)
                    .HasColumnName("landtype_desc");

              
                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                   .HasDefaultValue(false)
                   .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");

            });

            modelBuilder.Entity<TblIdmIrLand>(entity =>
            {
                entity.HasKey(e => e.IrlId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_ir_land");

                entity.HasIndex(e => e.LoanAcc, "FK_tbl_idm_ir_land_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_ir_land_offc_cdtab_offc_cdtab");

                entity.Property(e => e.IrlId)
                    .HasColumnName("irl_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.IrlIno)
                    .HasColumnName("irl_ino");

                entity.Property(e => e.IrlIdt)
                    .HasColumnName("irl_idt");

                entity.Property(e => e.IrlRdt)
                   .HasColumnName("irl_rdt");

                entity.Property(e => e.IrlArea)
                   .HasColumnName("irl_area");

                entity.Property(e => e.IrlAreaIn)
                   .HasColumnName("irl_areain");

                entity.Property(e => e.IrlLandCost)
                    .HasColumnName("irl_landcost");

                entity.Property(e => e.IrlDevCost)
                  .HasColumnName("irl_devcost");

                entity.Property(e => e.IrlLandTy)
                  .HasColumnName("irl_landty");

                entity.Property(e => e.IrlRem)
                  .HasColumnName("irl_rem");

                entity.Property(e => e.IrlSecValue)
                  .HasColumnName("irl_secvalue");

                entity.Property(e => e.IrlRelStat)
                  .HasColumnName("irl_rel_stat");

                entity.Property(e => e.IrlLandFinance)
                  .HasColumnName("irl_land_finance");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");

                //entity.HasOne(e => e.OffcCdtab)
                //  .WithOne(b => b.TblIdmIrLand)
                //  .HasForeignKey<TblIdmIrLand>(b => b.OffcCd);
            });

            // For mapping of the table TblIdmlrFurn
            modelBuilder.Entity<TblIdmIrFurn>(entity =>
            {
                entity.HasKey(e => e.IrfId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_ir_furn");

                entity.HasIndex(e => e.LoanAcc, "FK_tbl_idm_ir_furn_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_ir_furn_offc_cdtab");

                entity.Property(e => e.IrfId)
                    .HasColumnName("irf_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.IrfIDT)
                    .HasColumnName("irf_idt");

                entity.Property(e => e.IrfRDT)
                    .HasColumnName("irf_rdt");

                entity.Property(e => e.IrfItem)
                   .HasColumnName("irf_item");

                entity.Property(e => e.IrfAmt)
                    .HasColumnName("irf_amt");

                entity.Property(e => e.IrfNo)
                    .HasColumnName("irf_no");

                entity.Property(e => e.IrfIno)
                    .HasColumnName("irf_ino");

                entity.Property(e => e.IrfAqrdStat)
                    .HasColumnName("irf_aqrd_stat");

                entity.Property(e => e.IrfSecAmt)
                    .HasColumnName("irf_secamt");

                entity.Property(e => e.IrfRelStat)
                    .HasColumnName("irf_rel_stat");

                entity.Property(e => e.IrfAamt)
                    .HasColumnName("irf_aamt");

                entity.Property(e => e.IrfTotalRelease)
                    .HasColumnName("irf_total_release");

                entity.Property(e => e.IrfItemDets)
                    .HasMaxLength(200)
                    .HasColumnName("irf_itemdets");

                entity.Property(e => e.IrfSupplier)
                    .HasMaxLength(200)
                    .HasColumnName("irf_supplier");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblIdmIrPlmc>(entity =>
            {
                entity.HasKey(e => e.IrPlmcId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_ir_plmc");

                entity.HasIndex(e => e.LoanAcc, "FK_tbl_idm_ir_plmc_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_ir_plmc_offc_cdtab");

                entity.Property(e => e.IrPlmcId)
                    .HasColumnName("irplmc_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.IrPlmcIno)
             .HasColumnName("irplmc_ino");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.IrPlmcIdt)
                    .HasColumnName("irplmc_idt");

                entity.Property(e => e.IrPlmcRdt)
                    .HasColumnName("irplmc_rdt");

                entity.Property(e => e.IrPlmcItem)
                   .HasColumnName("irplmc_item");

                entity.Property(e => e.IrPlmcAmt)
                   .HasColumnName("irplmc_amt");

                entity.Property(e => e.IrPlmcNo)
                    .HasColumnName("irplmc_no");

                entity.Property(e => e.IrPlmcAcqrdStatus)
                    .HasColumnName("irplmc_aqrd_stat");

                entity.Property(e => e.IrPlmcFlg)
                    .HasColumnName("irplmc_flg");

                entity.Property(e => e.IrPlmcSecAmt)
                    .HasColumnName("irplmc_secamt");

                entity.Property(e => e.IrPlmcAcqrdIndicator)
                    .HasColumnName("irplmc_aqrd_indicator");

                entity.Property(e => e.IrPlmcRelseStat)
                    .HasColumnName("irplmc_rel_stat");

                entity.Property(e => e.IrPlmcAamt)
                    .HasColumnName("irplmc_aamt");

                entity.Property(e => e.IrPlmcItemDets)
                    .HasColumnName("irplmc_itemdets");

                entity.Property(e => e.IrPlmcSupplier)
                    .HasColumnName("irplmc_supplier");

                entity.Property(e => e.IrPlmcTotalRelease)
                    .HasColumnName("irplmc_total_release");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");
            });

            modelBuilder.Entity<TblIdmBuildingAcquisitionDetails>(entity =>
            {
                entity.HasKey(e => e.Irbid)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_id_bldg");

                entity.HasIndex(e => e.LoanAcc, "FK_tbl_idm_id_bldg_tbl_app_loan_mast");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_id_bldg_offc_cdtab");

                entity.Property(e => e.Irbid)
                .HasColumnName("irb_id");

                entity.Property(e => e.IrbIno)
               .HasColumnName("irb_ino");

                entity.Property(e => e.LoanAcc)
                .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                .HasColumnName("offc_cd");

                entity.Property(e => e.IrbIdt)
                .HasColumnName("irb_idt");

                entity.Property(e => e.IrbItem)
                .HasColumnName("irb_itm");

                entity.Property(e => e.IrbArea)
                .HasColumnName("irb_area");

                entity.Property(e => e.IrbValue)
                .HasColumnName("irb_value");

                entity.Property(e => e.IrbNo)
                .HasColumnName("irb_no");

                entity.Property(e => e.IrbStatus)
                .HasColumnName("irb_stat");

                entity.Property(e => e.IrbSecValue)
                .HasColumnName("irb_secvalue");

                entity.Property(e => e.IrbRelStat)
                .HasColumnName("irb_rel_stat");

                entity.Property(e => e.IrbAPArea)
                .HasColumnName("irb_aarea");

                entity.Property(e => e.IrbATCost)
                .HasColumnName("irb_avalue");

                entity.Property(e => e.IrbPercentage)
                .HasColumnName("irb_percent");

                entity.Property(e => e.IrbBldgConstStatus)
                .HasColumnName("irb_bldgconst_status");

                entity.Property(e => e.IrbBldgDetails)
                .HasColumnName("irb_bldg_details");

                entity.Property(e => e.IrbUnitCost)
                .HasColumnName("irb_unit_cost");

                entity.Property(e => e.RoofType)
                .HasColumnName("rf_ty");

                entity.Property(e => e.IrbCost)
                .HasColumnName("irb_cost");

                entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                .HasColumnName("unique_id");


            });

            modelBuilder.Entity<IdmDsbdets>(entity =>
            {
                entity.HasKey(e => e.DsbdetsID)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_dsb_dets");

                entity.HasIndex(e => e.LoanAcc, "idm_dsb_dets_id_fk1_idx");

                entity.HasIndex(e => e.OffcCd, "idm_dsb_dets_idfk2_idx");

                entity.Property(e => e.DsbdetsID)
                .HasColumnName("dsb_dets_id");

                entity.Property(e => e.LoanAcc)
                .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                .HasColumnName("offc_cd");

                entity.Property(e => e.DsbNo)
               .HasColumnName("dsb_no");

                entity.Property(e => e.DsbDt)
                .HasColumnName("dsb_dt");

                entity.Property(e => e.DsbAmt)
                .HasColumnName("dsb_amt");

                entity.Property(e => e.DsbAcd)
                .HasColumnName("dsb_acd");

                  entity.Property(e => e.PropAmt)
                .HasColumnName("prop_amt");

                entity.Property(e => e.DsbEstAmt)
                .HasColumnName("dsb_est_amt");

                entity.Property(e => e.SecConsideredFRelease)
                .HasColumnName("sec_considered_f_release");

                entity.Property(e => e.SecInspection)
                .HasColumnName("sec_inspection");

                entity.Property(e => e.MarginRetained)
                .HasColumnName("Margin_retained");

                entity.Property(e => e.AlocAmt)
                .HasColumnName("aloc_amt");

                entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                .HasColumnName("unique_id");


            });

            modelBuilder.Entity<CodeTable>(entity =>
            {

                entity.HasKey(e => e.Id)
                    .HasName("PRIMARY");

                entity.ToTable("code_table");

                entity.Property(e => e.Id)
                    .HasColumnName("id");

                entity.Property(e => e.ModuleName)
                        .HasColumnName("module_name");


                entity.Property(e => e.CodeType)
                        .HasColumnName("code_type");

                entity.Property(e => e.CodeName)
                        .HasColumnName("code_name");

                entity.Property(e => e.CodeValue)
                        .HasColumnName("code_value");

                entity.Property(e => e.DisplaySequence)
                        .HasColumnName("display_sequence");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true)
                      .HasColumnName("is_active");

            });

            //UC08
            modelBuilder.Entity<TblIdmDisbProp>(entity =>
            {
                entity.HasKey(e => e.PropId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_disb_prop");


                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_disb_prop_offc_cdtab");

                entity.Property(e => e.PropId)
                .HasColumnName("prop_id");

                entity.Property(e => e.LoanAcc)
                .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                .HasColumnName("offc_cd");

                entity.Property(e => e.PropNumber)
                .HasColumnName("prop_number");

                entity.Property(e => e.PropDate)
                .HasColumnName("prop_date");

                entity.Property(e => e.PropDept)
                .HasColumnName("prop_dept");

                entity.Property(e => e.PropLoanType)
                .HasColumnName("prop_loan_type");

                entity.Property(e => e.PropSancAmount)
                .HasColumnName("prop_sanc_amt");

                entity.Property(e => e.PropDisbAmount)
                .HasColumnName("prop_disb_amt");

                entity.Property(e => e.PropRecAmount)
                .HasColumnName("prop_rec_amt");

                entity.Property(e => e.PropStatusFlg)
                .HasColumnName("prop_status_flg");

                entity.Property(e => e.PropFdsbFlg)
                .HasColumnName("prop_fdsb_flg");

                entity.Property(e => e.PropReltyFlg)
                .HasColumnName("prop_relty_flg");


                entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                .HasColumnName("unique_id");


            });

            modelBuilder.Entity<TblIdmReleDetls>(entity =>
            {
                entity.HasKey(e => e.ReleId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_rele_detls");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_rele_detls_offc_cdtab");
                entity.HasIndex(e => e.PropNumber, "FK_tbl_idm_rele_detls_tbl_idm_disb_prop");

                entity.Property(e => e.ReleId)
                .HasColumnName("rele_id");

                entity.Property(e => e.LoanAcc)
                .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                .HasColumnName("offc_cd");

                entity.Property(e => e.PropNumber)
                .HasColumnName("prop_number");

                entity.Property(e => e.ReleDueAmount)
                .HasColumnName("rele_due_amt");

                entity.Property(e => e.ReleAtParAmount)
                .HasColumnName("rele_at_par_amt");

                entity.Property(e => e.ReleAtParCharges)
                .HasColumnName("rele_at_par_charges");

                entity.Property(e => e.ReleBnkChg)
                .HasColumnName("rele_bnk_chg");

                entity.Property(e => e.ReleUpFrtAmount)
                .HasColumnName("rele_up_frt_amt");

                entity.Property(e => e.ReleDocChg)
                .HasColumnName("rele_doc_chg");

                entity.Property(e => e.ReleComChg)
                .HasColumnName("rele_com_chg");

                entity.Property(e => e.ReleFdAmount)
                .HasColumnName("rele_fd_amt");

                entity.Property(e => e.ReleFdGlcd)
                .HasColumnName("rele_fd_glcd");

                entity.Property(e => e.ReleOthAmount)
                .HasColumnName("rele_oth_amt");

                entity.Property(e => e.ReleOthGlcd)
                .HasColumnName("rele_oth_glcd");

                entity.Property(e => e.ReleAdjAmount)
                .HasColumnName("rele_adj_amt");

                entity.Property(e => e.ReleAdjRecSeq)
                .HasColumnName("rele_adj_rec_seq");

                entity.Property(e => e.ReleAddUpFrtAmount)
                .HasColumnName("rele_add_up_frt_amt");

                entity.Property(e => e.ReleAddlafdAmount)
                .HasColumnName("rele_addlafd_amt");

                entity.Property(e => e.ReleSertaxAmount)
                .HasColumnName("rele_sertax_amt");

                entity.Property(e => e.ReleCersai)
                .HasColumnName("rele_cersai");

                entity.Property(e => e.ReleSwachcess)
                .HasColumnName("rele_swachcess");


                entity.Property(e => e.Relekrishikalyancess)
                .HasColumnName("rele_krishikalyancess");

                entity.Property(e => e.ReleCollGuaranteeFee)
                .HasColumnName("rele_coll_guarantee_fee");

                entity.Property(e => e.ReleNumber)
                .HasColumnName("rele_number");

                entity.Property(e => e.ReleDate)
                .HasColumnName("rele_date");

                entity.Property(e => e.ReleAmount)
                .HasColumnName("rele_amt");

                entity.Property(e => e.IsActive)
                .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                .HasColumnName("unique_id");

                entity.Property(e => e.AddlAmt1)
                .HasColumnName("addl_amt1"); // add amounts here 

                entity.Property(e => e.AddlAmt2)
                .HasColumnName("addl_amt2");

                entity.Property(e => e.AddlAmt3)
                .HasColumnName("addl_amt3");

                entity.Property(e => e.AddlAmt4)
                .HasColumnName("addl_amt4");

                entity.Property(e => e.AddlAmt5)
                .HasColumnName("addl_amt5");

                entity.HasOne(e => e.TblIdmDisbProp)
                  .WithOne(b => b.TblIdmReleDetls)
                  .HasForeignKey<TblIdmReleDetls>(b => b.PropNumber);
            });

            modelBuilder.Entity<TblIdmBenfDet>(entity =>
            {

                entity.HasKey(e => e.BenfId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_idm_benf_detls");

                entity.HasIndex(e => e.OffcCd, "FK_tbl_idm_benf_detls_offc_cdtab");

                entity.Property(e => e.BenfId)
                    .HasColumnName("benf_id");

                entity.Property(e => e.LoanAcc)
                        .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                        .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                        .HasColumnName("offc_cd");

                entity.Property(e => e.BenfNumber)
                        .HasColumnName("benf_number");

                entity.Property(e => e.BenfDate)
                        .HasColumnName("benf_date");

                entity.Property(e => e.BenfDept)
                        .HasColumnName("benf_dept");

                entity.Property(e => e.BenfType)
                        .HasColumnName("benf_type");

                entity.Property(e => e.BenfName)
                        .HasColumnName("benf_name");

                entity.Property(e => e.BenfCode)
                        .HasColumnName("benf_code");

                entity.Property(e => e.BenfAmt)
                        .HasColumnName("benf_amt");

                entity.Property(e => e.BenfInstType)
                        .HasColumnName("benf_inst_type");

                entity.Property(e => e.BenfInstFlag)
                        .HasColumnName("benf_inst_flag");

                entity.Property(e => e.BenfRecSeq)
                        .HasColumnName("benf_rec_seq");

                entity.Property(e => e.DDAtparLoc)
                        .HasColumnName("dd_atpar_loc");

                entity.Property(e => e.BenfRelAdjAmt)
                        .HasColumnName("benf_reladj_amt");

                entity.Property(e => e.BenfRtgsAcNo)
                        .HasColumnName("benf_rtgs_acno");

                entity.Property(e => e.BenfRtgsIfsc)
                        .HasColumnName("benf_rtgs_ifsc");

                entity.Property(e => e.BenfRtgsBank)
                        .HasColumnName("benf_rtgs_bank");

                entity.Property(e => e.BenfRtgsBnkBranch)
                        .HasColumnName("benf_rtgs_bkbranch");

                entity.Property(e => e.BenfRtgsBnkCity)
                        .HasColumnName("benf_rtgs_bkcity");

                entity.Property(e => e.BenfRtgsChqNo)
                        .HasColumnName("benf_rtgs_cheqno");

                entity.Property(e => e.BenfRtgsChqDt)
                        .HasColumnName("benf_rtgs_cheqdt");

                entity.Property(e => e.IsActive)
                      .HasDefaultValue(true)
                      .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");

            });

            /// <summary>
            ///  Author: Gagana K; Module:EntryOfOtherDebits; Date:27/10/2022
            /// </summary>
            modelBuilder.Entity<IdmOthdebitsDetails>(entity =>
            {
                entity.HasKey(e => e.OthdebitDetId)
                    .HasName("PRIMARY");

                entity.ToTable("idm_othdebits_details");

                entity.HasIndex(e => e.LoanAcc, "FK_idm_othdebits_details_tbl_app_loan_mast");

                entity.HasIndex(e => e.DsbOthdebitId, "FK_idm_othdebits_details_idm_othdebits_mast");

                entity.Property(e => e.OthdebitDetId)
                    .HasColumnName("othdebit_det_id");

                entity.Property(e => e.LoanAcc)
                    .HasColumnName("loan_acc");

                entity.Property(e => e.LoanSub)
                    .HasColumnName("loan_sub");

                entity.Property(e => e.OffcCd)
                    .HasColumnName("offc_cd");

                entity.Property(e => e.DsbOthdebitId)
                    .HasColumnName("dsb_othdebit_id");

                entity.Property(e => e.OthdebitAmt)
                    .HasColumnName("othdebit_amt");

                entity.Property(e => e.OthdebitGst)
                   .HasColumnName("othdebit_gst");

                entity.Property(e => e.OthdebitTaxes)
                   .HasColumnName("othdebit_taxes");

                entity.Property(e => e.OthdebitDuedate)
                    .HasColumnName("othdebit_duedate");

                entity.Property(e => e.OthdebitTotal)
                    .HasColumnName("othdebit_total");

                entity.Property(e => e.OthdebitRemarks)
                    .HasColumnName("othdebit_remarks");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreateBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");

                entity.Property(e => e.IsSubmitted)
                    .HasColumnName("is_submitted");
            });

            /// <summary>
            ///  Author: Gagana K; Module:EntryOfOtherDebits; Date:27/10/2022
            /// </summary>
            modelBuilder.Entity<IdmOthdebitsMast>(entity =>
            {
                entity.HasKey(e => e.DsbOthdebitId)
                    .HasName("PRIMARY");

                entity.ToTable("idm_othdebits_mast");

                entity.Property(e => e.DsbOthdebitId)
                    .HasColumnName("dsb_othdebit_id");

                entity.Property(e => e.DsbOthdebitDesc)
                    .HasColumnName("dsb_othdebit_desc");

                entity.Property(e => e.DsbOthdebitDisSeq)
                    .HasColumnName("dsb_othdebit_dis_seq");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");
                
            });

            /// <summary>
            ///  Author: Gagana K; Module:ChangeLocation; Date:16/11/2022
            /// </summary>
            modelBuilder.Entity<TblPincodeMaster>(entity =>
            {
                entity.HasKey(e => e.PincodeRowId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_pincode_master");

                entity.Property(e => e.PincodeRowId)
                    .HasColumnName("pincode_rowid");

                entity.Property(e => e.PincodeCd)
                    .HasColumnName("pincode_cd");

                entity.Property(e => e.PincodeState)
                    .HasColumnName("pincode_state");

                entity.Property(e => e.PincodeDistrict)
                    .HasColumnName("pincode_district");

                entity.Property(e => e.PincodeDistrictCd)
                    .HasColumnName("pincode_district_cd");

                entity.Property(e => e.IsActive)
                    .HasDefaultValue(true)
                    .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                    .HasDefaultValue(false)
                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                    .HasMaxLength(50)
                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                    .HasMaxLength(50)
                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                    .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                    .HasColumnName("modified_date");

            });

            modelBuilder.Entity<TblUMOMaster>(entity =>
            {
                entity.HasKey(e => e.UmoId)
                    .HasName("PRIMARY");

                entity.ToTable("tbl_uom_mast");

               
                entity.Property(e => e.UmoId)
                  .HasColumnName("uom_id");

                entity.Property(e => e.UmoCode)
                    .HasColumnName("uom_code");


                entity.Property(e => e.UmoDesc)
                   .HasColumnName("uom_desc");


                entity.Property(e => e.UniqueId)
                    .HasColumnName("unique_id");


                entity.Property(e => e.IsActive)
                                  .HasDefaultValue(true)
                                   .HasColumnName("is_active");

                entity.Property(e => e.IsDeleted)
                                    .HasDefaultValue(false)
                                    .HasColumnName("is_deleted");

                entity.Property(e => e.CreatedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("created_by");

                entity.Property(e => e.ModifiedBy)
                                    .HasMaxLength(50)
                                    .HasColumnName("modified_by");

                entity.Property(e => e.CreatedDate)
                                .HasColumnName("created_date");

                entity.Property(e => e.ModifiedDate)
                                    .HasColumnName("modified_date");

            });


            modelBuilder.Entity<TblProcureMast>(entity =>
            {
                entity.HasKey(e => e.ProcureId)
                   .HasName("PRIMARY");

                entity.ToTable("tbl_procure_mast");

                entity.Property(e => e.ProcureId)
                    .HasColumnName("procure_id");

                entity.Property(e => e.ProcureDesc)
                        .HasColumnName("procure_desc");
            });



            modelBuilder.Entity<TblCurrencyMast>(entity =>
            {
                entity.HasKey(e => e.CurrencyId)
                   .HasName("PRIMARY");

                entity.ToTable("tbl_currency_mast");

                entity.Property(e => e.CurrencyId)
                    .HasColumnName("currency_id");

                entity.Property(e => e.CurrencyDesc)
                        .HasColumnName("currency_desc");
            });


            modelBuilder.Entity<TblMachineryStatus>(entity =>
            {
                entity.HasKey(e => e.MacStatus)
                   .HasName("PRIMARY");

                entity.ToTable("tbl_machinery_status");

                entity.Property(e => e.MacStatus)
                    .HasColumnName("mac_status");

                entity.Property(e => e.MacDets)
                        .HasColumnName("mac_dets");
            });

              modelBuilder.Entity<TblDeptMaster>(entity =>
            {
                entity.HasKey(e => e.DeptCode)
                   .HasName("PRIMARY");

                entity.ToTable("tbl_dept_master");

                entity.Property(e => e.DeptCode)
                    .HasColumnName("dept_code");

                entity.Property(e => e.DeptName)
                        .HasColumnName("dept_name");
            });

               modelBuilder.Entity<TblDsbChargeMap>(entity =>
            {
                entity.HasKey(e => e.DsbOthdebitId)
                   .HasName("PRIMARY");

                entity.ToTable("tbl_dsb_charge_map");

                entity.Property(e => e.DsbOthdebitId)
                    .HasColumnName("dsb_othdebit_id");

                entity.Property(e => e.DataFieldName)
                        .HasColumnName("data_field_name");
            });

            OnModelCreatingPartial(modelBuilder);
        }
        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
