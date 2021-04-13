USE [HRViabilityPortal]
GO
/****** Object:  Table [dbo].[AuditLogs]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[AuditLogs](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[UserId] [varchar](max) NULL,
	[ActivityPerformed] [varchar](max) NULL,
	[UserName] [varchar](max) NULL,
	[DateOfActivity] [datetime] NULL,
	[IpAddress] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BaiMuajjalDocumentsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BaiMuajjalDocumentsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[documentName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BaiMuajjalFormsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BaiMuajjalFormsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[formName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BaiMuajjalGradesTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BaiMuajjalGradesTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gradeName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BMDetails]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[BMDetails](
	[staffId] [nvarchar](50) NOT NULL,
	[staffName] [nvarchar](max) NOT NULL,
	[branchName] [nvarchar](max) NOT NULL,
	[branchCode] [nvarchar](50) NOT NULL,
	[emailAddress] [nvarchar](max) NOT NULL,
 CONSTRAINT [PK_BMDetails] PRIMARY KEY CLUSTERED 
(
	[staffId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
/****** Object:  Table [dbo].[BranchDocumentsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BranchDocumentsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[documentName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BranchFormsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BranchFormsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[formName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[BranchGradesTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[BranchGradesTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gradeName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[EmailAccount]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[EmailAccount](
	[emailID] [int] IDENTITY(1,1) NOT NULL,
	[emailName] [nvarchar](50) NOT NULL,
	[emailAddress] [nvarchar](50) NOT NULL,
	[emailPassword] [nvarchar](100) NOT NULL,
	[emailGateway] [nvarchar](50) NOT NULL,
	[emailUserName] [nvarchar](150) NOT NULL,
	[emailStatus] [int] NOT NULL,
	[emailINTBK] [int] NULL,
 CONSTRAINT [PK_EmailAccount] PRIMARY KEY CLUSTERED 
(
	[emailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[EmailBox]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[EmailBox](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[EmailAddress] [varchar](50) NULL,
	[EmailContent] [varchar](max) NULL,
	[HasAttachment] [int] NULL,
	[Attachment] [varchar](50) NULL,
	[EmailStatus] [varchar](5) NULL,
	[EntryDate] [datetime] NULL,
	[SentDate] [datetime] NULL,
	[Platform] [varchar](50) NULL,
	[FromAddress] [varchar](50) NULL,
	[Subject] [varchar](150) NULL,
 CONSTRAINT [PK_EmailBox] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[Facility]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[Facility](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[facilityName] [varchar](max) NOT NULL,
	[confirmedStaff] [varchar](3) NULL,
	[unconfirmedStaff] [varchar](3) NULL,
	[permanentStaff] [varchar](3) NULL,
	[contractStaff] [varchar](3) NULL,
	[expectedLengthOfService] [int] NULL,
	[percentageRate] [numeric](18, 0) NULL,
	[staffBranchPercentageRate] [numeric](18, 0) NULL,
	[minimumAnnualGrossAllowance] [numeric](18, 0) NULL,
	[minimumAnnualHousingAllowance] [numeric](18, 0) NULL,
	[maximumAmountLimit] [numeric](18, 0) NULL,
	[totalDeductionFromSalary] [varchar](10) NULL,
	[minimumTenor] [int] NULL,
	[maximumTenor] [int] NULL,
	[undertakingFormNeeded] [varchar](3) NULL,
	[supervisorEndorsementNeeded] [varchar](3) NULL,
	[searchAuthorityNeeded] [varchar](3) NULL,
	[active] [varchar](1) NULL,
	[facilityComments] [varchar](max) NULL,
	[document1] [varchar](max) NULL,
	[document2] [varchar](max) NULL,
	[document3] [varchar](max) NULL,
	[document4] [varchar](max) NULL,
	[document5] [varchar](max) NULL,
	[document6] [varchar](max) NULL,
	[document7] [varchar](max) NULL,
	[document8] [varchar](max) NULL,
	[document9] [varchar](max) NULL,
	[document10] [varchar](max) NULL,
	[form1] [varchar](max) NULL,
	[form2] [varchar](max) NULL,
	[form3] [varchar](max) NULL,
	[form4] [varchar](max) NULL,
	[form5] [varchar](max) NULL,
	[form6] [varchar](max) NULL,
	[form7] [varchar](max) NULL,
	[form8] [varchar](max) NULL,
	[form9] [varchar](max) NULL,
	[form10] [varchar](max) NULL,
	[facilityRulesComments] [varchar](max) NULL,
	[facilityRulesSet] [varchar](1) NULL,
	[maximumAmountLimitOption] [varchar](max) NULL,
	[mdApproval] [varchar](3) NULL,
	[downPaymentRequired] [varchar](3) NULL,
	[dhApproval] [varchar](3) NULL,
	[customField1] [varchar](max) NULL,
	[customField2] [varchar](max) NULL,
	[customField3] [varchar](max) NULL,
	[customField4] [varchar](max) NULL,
	[customField5] [varchar](max) NULL,
	[customField6] [varchar](max) NULL,
	[customField7] [varchar](max) NULL,
	[customField8] [varchar](max) NULL,
	[customField9] [varchar](max) NULL,
	[customField10] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FacilityDocuments]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacilityDocuments](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[documentName] [varchar](max) NOT NULL,
	[active] [varchar](1) NOT NULL,
	[isSelected] [bit] NULL,
	[comments] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FacilityForms]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacilityForms](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[formName] [varchar](max) NOT NULL,
	[active] [varchar](1) NOT NULL,
	[isSelected] [bit] NULL,
	[comments] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[FacilityUndertaking]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[FacilityUndertaking](
	[id] [int] NOT NULL,
	[undertakingType] [varchar](50) NULL,
	[active] [varchar](3) NULL,
	[isSelected] [bit] NULL,
	[comments] [varchar](max) NULL,
 CONSTRAINT [PK_FacilityUndertaking] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HomeFinanceDocumentsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HomeFinanceDocumentsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[documentName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HomeFinanceFormsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HomeFinanceFormsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[formName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HomeFinanceGradesTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HomeFinanceGradesTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gradeName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HRBranchesMaster]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HRBranchesMaster](
	[BranchCode] [int] NOT NULL,
	[BranchName] [varchar](max) NULL,
 CONSTRAINT [PK_HRBranchesMaster] PRIMARY KEY CLUSTERED 
(
	[BranchCode] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HRFacilityMaster]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HRFacilityMaster](
	[ID] [bigint] IDENTITY(1,1) NOT NULL,
	[requestReferenceNumber] [varchar](max) NOT NULL,
	[staffId] [varchar](50) NULL,
	[accountNumber] [varchar](10) NOT NULL,
	[cif] [int] NULL,
	[accountName] [varchar](max) NOT NULL,
	[facilityName] [varchar](max) NULL,
	[salaryGrade] [varchar](max) NULL,
	[mobileNumber] [varchar](11) NULL,
	[maritalStatus] [varchar](50) NULL,
	[dateOfBirth] [varchar](50) NULL,
	[staffEmailAddress] [varchar](max) NULL,
	[state] [varchar](50) NULL,
	[natureOfEmployment] [varchar](max) NOT NULL,
	[numberOfMonthsInService] [int] NULL,
	[jobFunction] [varchar](max) NULL,
	[branch] [varchar](max) NULL,
	[branchCode] [varchar](50) NULL,
	[department] [varchar](max) NULL,
	[grossAnnualSalary] [numeric](18, 0) NOT NULL,
	[netMonthlySalary] [numeric](18, 0) NOT NULL,
	[annualHousingAllowance] [numeric](18, 0) NULL,
	[outstandingLoanAmount] [numeric](18, 0) NULL,
	[noOfMonthsRemainingOnfacility] [int] NULL,
	[amountRequested] [numeric](18, 0) NOT NULL,
	[downPaymentContribution] [numeric](18, 0) NULL,
	[repaymentAmount] [numeric](18, 0) NULL,
	[percentageRate] [numeric](18, 0) NULL,
	[tenor] [int] NOT NULL,
	[loanPurpose] [varchar](max) NOT NULL,
	[assetDescription] [varchar](max) NULL,
	[vendorsName] [varchar](max) NULL,
	[vendorsAddress] [varchar](max) NULL,
	[vendorsPhoneNumber] [varchar](50) NULL,
	[locationOfAsset] [varchar](max) NULL,
	[vehicleModel] [varchar](max) NULL,
	[yearOfManufacture] [varchar](max) NULL,
	[modeOfDelivery] [varchar](max) NULL,
	[quotationEnclosed] [varchar](max) NULL,
	[typeOfProperty] [varchar](50) NULL,
	[propertyDescription] [varchar](max) NULL,
	[titleDocumentType] [varchar](max) NULL,
	[nameOfDeveloper] [varchar](max) NULL,
	[addressOfDeveloper] [varchar](max) NULL,
	[phoneNumberOfDeveloper] [varchar](50) NULL,
	[locationOfProperty] [varchar](max) NULL,
	[serviceDescription] [varchar](max) NULL,
	[serviceProviderName] [varchar](max) NULL,
	[serviceProviderAddress] [varchar](max) NULL,
	[servicePhoneNumber] [varchar](50) NULL,
	[requestDate] [datetime] NULL,
	[requestStatus] [varchar](50) NULL,
	[initiator] [varchar](max) NULL,
	[initiatorTimestamp] [datetime] NULL,
	[staffSupervisor] [varchar](50) NULL,
	[hrApprover1] [varchar](max) NULL,
	[hrApprover1ApproveReject] [varchar](max) NULL,
	[hrApprover1Comment] [varchar](max) NULL,
	[hrApprover1Timestamp] [datetime] NULL,
	[hrApprover2] [varchar](max) NULL,
	[hrApprover2Timestamp] [datetime] NULL,
	[hrApprover2ApproveReject] [varchar](max) NULL,
	[hrApprover2Comment] [varchar](max) NULL,
	[bmBranchOfRequest] [varchar](max) NULL,
	[bmBranchOfRequestTimestamp] [datetime] NULL,
	[bmBranchOfRequestApproveReject] [varchar](max) NULL,
	[bmBranchOfRequestComment] [varchar](max) NULL,
	[mdApprover] [varchar](max) NULL,
	[mdApproverTimestamp] [datetime] NULL,
	[mdApproverApproveReject] [varchar](max) NULL,
	[mdApproverComment] [varchar](max) NULL,
	[fileName1] [varchar](max) NULL,
	[fileContentType1] [nvarchar](200) NULL,
	[fileData1] [nvarchar](max) NULL,
	[fileName2] [varchar](max) NULL,
	[fileContentType2] [nvarchar](200) NULL,
	[fileData2] [nvarchar](max) NULL,
	[fileName3] [varchar](max) NULL,
	[fileContentType3] [nvarchar](200) NULL,
	[fileData3] [nvarchar](max) NULL,
	[fileName4] [varchar](max) NULL,
	[fileContentType4] [nvarchar](200) NULL,
	[fileData4] [nvarchar](max) NULL,
	[fileName5] [varchar](max) NULL,
	[fileContentType5] [nvarchar](200) NULL,
	[fileData5] [nvarchar](max) NULL,
	[fileName6] [varchar](max) NULL,
	[fileContentType6] [nvarchar](200) NULL,
	[fileData6] [nvarchar](max) NULL,
	[hrApprover1RejectComment] [varchar](max) NULL,
	[hrApprover2RejectComment] [varchar](max) NULL,
	[mdApproverRejectComment] [varchar](max) NULL,
	[bmApproverRejectComment] [varchar](max) NULL,
	[dhApproverRejectComment] [varchar](max) NULL,
	[dhApprover] [varchar](max) NULL,
	[dhApproverTimestamp] [datetime] NULL,
	[dhApproverApproveReject] [varchar](50) NULL,
	[dhApproverComment] [varchar](max) NULL,
	[hasExistingVehicleLoan] [varchar](3) NULL,
	[hasExistingMortgageLoan] [varchar](3) NULL,
	[dateOfEmployment] [varchar](50) NULL,
	[customField1] [varchar](max) NULL,
	[customField1Data] [varchar](max) NULL,
	[customField2] [varchar](max) NULL,
	[customField2Data] [varchar](max) NULL,
	[customField3] [varchar](max) NULL,
	[customField3Data] [varchar](max) NULL,
	[customField4] [varchar](max) NULL,
	[customField4Data] [varchar](max) NULL,
	[customField5] [varchar](max) NULL,
	[customField5Data] [varchar](max) NULL,
	[customField6] [varchar](max) NULL,
	[customField6Data] [varchar](max) NULL,
	[customField7] [varchar](max) NULL,
	[customField7Data] [varchar](max) NULL,
	[customField8] [varchar](max) NULL,
	[customField8Data] [varchar](max) NULL,
	[customField9] [varchar](max) NULL,
	[customField9Data] [varchar](max) NULL,
	[customField10] [varchar](max) NULL,
	[customField10Data] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[HRPayroll]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[HRPayroll](
	[Id] [int] IDENTITY(1,1) NOT NULL,
	[GradeName] [varchar](max) NULL,
	[AnnualGrossPackage] [varchar](max) NULL,
	[AnnualHousingAllowance] [varchar](max) NULL,
	[MonthlyGrossPackage] [varchar](max) NULL,
	[MonthlyNetPackage] [varchar](max) NULL,
	[PensionsContribution] [varchar](max) NULL,
	[PAYE] [varchar](max) NULL,
	[NHF] [varchar](max) NULL,
	[TotalAnnualDeduction] [varchar](max) NULL,
	[TotalMonthlyDeduction] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IjaraServiceDocumentsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IjaraServiceDocumentsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[documentName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IjaraServiceFormsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IjaraServiceFormsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[formName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[IjaraServiceGradesTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[IjaraServiceGradesTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gradeName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MurabahaDocumentsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MurabahaDocumentsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[documentName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MurabahaFormsTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MurabahaFormsTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[formName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[MurabahaGradesTable]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[MurabahaGradesTable](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gradeName] [varchar](max) NULL,
	[active] [varchar](1) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
/****** Object:  Table [dbo].[ReqHistory]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[ReqHistory](
	[sn] [int] IDENTITY(1,1) NOT NULL,
	[ReqId] [nvarchar](50) NULL,
	[Initiator] [nvarchar](50) NULL,
	[ActionPerformed] [nvarchar](500) NULL,
	[ActionDateTime] [datetime] NULL,
 CONSTRAINT [PK_ReqHistory] PRIMARY KEY CLUSTERED 
(
	[sn] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[RequestStatus]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[RequestStatus](
	[ReqStatId] [int] IDENTITY(1,1) NOT NULL,
	[ReqStatus] [nvarchar](50) NULL,
 CONSTRAINT [PK_RequestStatus] PRIMARY KEY CLUSTERED 
(
	[ReqStatId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
/****** Object:  Table [dbo].[StaffGradesSalary]    Script Date: 10/12/2019 7:54:59 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_PADDING ON
GO
CREATE TABLE [dbo].[StaffGradesSalary](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[gradeName] [varchar](max) NOT NULL,
	[netMonthlySalary] [numeric](18, 0) NOT NULL,
	[grossAnnualSalary] [numeric](18, 0) NOT NULL,
	[annualHousingAllowance] [numeric](18, 0) NULL,
	[isSelected] [bit] NULL,
	[active] [varchar](3) NULL,
	[comments] [varchar](max) NULL,
PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]

GO
SET ANSI_PADDING OFF
GO
ALTER TABLE [dbo].[EmailBox] ADD  CONSTRAINT [DF_EmailBox_HasAttachment]  DEFAULT ((0)) FOR [HasAttachment]
GO
ALTER TABLE [dbo].[EmailBox] ADD  CONSTRAINT [DF_EmailBox_EmailStatus]  DEFAULT ('N') FOR [EmailStatus]
GO
