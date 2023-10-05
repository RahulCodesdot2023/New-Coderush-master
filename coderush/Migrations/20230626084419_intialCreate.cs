using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodesDotHRMS.Migrations
{
    public partial class intialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    UserName = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(maxLength: 256, nullable: true),
                    Email = table.Column<string>(maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: true),
                    SecurityStamp = table.Column<string>(nullable: true),
                    ConcurrencyStamp = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(nullable: false),
                    TwoFactorEnabled = table.Column<bool>(nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(nullable: true),
                    LockoutEnabled = table.Column<bool>(nullable: false),
                    AccessFailedCount = table.Column<int>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EmpCode = table.Column<int>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    JoiningDate = table.Column<DateTime>(nullable: true),
                    DOB = table.Column<DateTime>(nullable: true),
                    Salary = table.Column<int>(nullable: true),
                    ProfilePicture = table.Column<string>(nullable: true),
                    isSuperAdmin = table.Column<bool>(nullable: true),
                    PaidLeave = table.Column<decimal>(nullable: true),
                    UnpaidLeave = table.Column<decimal>(nullable: true),
                    Pl = table.Column<string>(nullable: true),
                    EmployeeType = table.Column<int>(nullable: true),
                    EndOfDate = table.Column<DateTime>(nullable: true),
                    AadharFront = table.Column<byte[]>(nullable: true),
                    AadharBack = table.Column<byte[]>(nullable: true),
                    PanCard = table.Column<byte[]>(nullable: true),
                    ProfilePic = table.Column<byte[]>(nullable: true),
                    PanCardNo = table.Column<string>(nullable: true),
                    AadharCardNo = table.Column<string>(nullable: true),
                    BankName = table.Column<string>(nullable: true),
                    BankBranchName = table.Column<string>(nullable: true),
                    AccountNumber = table.Column<string>(nullable: true),
                    IFSCode = table.Column<string>(nullable: true),
                    IsWFH = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CandidateMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Technologies = table.Column<int>(nullable: false),
                    FileUpload = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    InterviewDate = table.Column<DateTime>(nullable: false),
                    PlaceOfInterview = table.Column<string>(nullable: false),
                    InterviewTime = table.Column<TimeSpan>(nullable: false),
                    InterviewDescription = table.Column<string>(nullable: false),
                    IsReject = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CandidateMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Note = table.Column<string>(nullable: false),
                    CandidateId = table.Column<int>(nullable: false),
                    NextFollowUpdate = table.Column<DateTime>(nullable: true),
                    Status = table.Column<int>(nullable: true),
                    LoginUser = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Configuration",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Key = table.Column<string>(nullable: true),
                    Value = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Configuration", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    email = table.Column<string>(nullable: true),
                    phoneno = table.Column<string>(nullable: true),
                    description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Credit",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(nullable: false),
                    Managername = table.Column<string>(nullable: true),
                    Creditdate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Credit", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Datamaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Type = table.Column<int>(nullable: false),
                    Text = table.Column<string>(nullable: false),
                    Description = table.Column<string>(maxLength: 60, nullable: false),
                    Isactive = table.Column<bool>(nullable: false),
                    Isdeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Datamaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EmployeeHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Salary = table.Column<float>(nullable: false),
                    Date = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmployeeHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExpenseMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExpName = table.Column<string>(maxLength: 50, nullable: false),
                    Exptype = table.Column<int>(nullable: false),
                    Amount = table.Column<string>(nullable: false),
                    ExpenseDate = table.Column<DateTime>(nullable: true),
                    Description = table.Column<string>(nullable: false),
                    FileUpload = table.Column<string>(nullable: true),
                    isactive = table.Column<bool>(nullable: false),
                    Isdelete = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    BankAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExpenseMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinanceHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FinanceID = table.Column<int>(nullable: false),
                    Bankname = table.Column<string>(nullable: true),
                    Accountownername = table.Column<string>(nullable: true),
                    Acnumber = table.Column<string>(nullable: true),
                    Branchname = table.Column<string>(nullable: true),
                    Ifsccode = table.Column<string>(nullable: true),
                    Chequephoto = table.Column<string>(nullable: true),
                    Atmphoto = table.Column<string>(nullable: true),
                    Projectname = table.Column<string>(nullable: true),
                    Handelby = table.Column<string>(nullable: true),
                    Isactive = table.Column<bool>(nullable: false),
                    Createdby = table.Column<string>(nullable: true),
                    Createddate = table.Column<DateTime>(nullable: true),
                    Updatedby = table.Column<string>(nullable: true),
                    Updateddate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinanceMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Bankname = table.Column<string>(nullable: false),
                    Accountownername = table.Column<string>(nullable: false),
                    Acnumber = table.Column<string>(nullable: false),
                    Branchname = table.Column<string>(nullable: false),
                    Ifsccode = table.Column<string>(nullable: false),
                    Chequephoto = table.Column<string>(nullable: false),
                    Atmphoto = table.Column<string>(nullable: false),
                    Projectname = table.Column<string>(nullable: false),
                    Handelby = table.Column<string>(nullable: true),
                    Isactive = table.Column<bool>(nullable: false),
                    Createdby = table.Column<string>(nullable: true),
                    Createddate = table.Column<DateTime>(nullable: true),
                    Updatedby = table.Column<string>(nullable: true),
                    Updateddate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinanceMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Hire",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(nullable: true),
                    Email = table.Column<string>(nullable: true),
                    country = table.Column<string>(nullable: true),
                    phoneNo = table.Column<string>(nullable: true),
                    Developers = table.Column<string>(nullable: true),
                    duration = table.Column<TimeSpan>(nullable: true),
                    description = table.Column<string>(nullable: true),
                    filename = table.Column<string>(nullable: true),
                    filebinary = table.Column<byte[]>(nullable: true),
                    filetype = table.Column<string>(nullable: true),
                    Createddate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hire", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HolidayList",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true),
                    Day = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: true),
                    Isdelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HolidayList", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "HRannouncement",
                columns: table => new
                {
                    announcementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HRannouncement", x => x.announcementId);
                });

            migrationBuilder.CreateTable(
                name: "InvoiceMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    Amount = table.Column<string>(nullable: false),
                    Duedate = table.Column<DateTime>(nullable: false),
                    InvoiceNumber = table.Column<string>(nullable: false),
                    PendingAmount = table.Column<string>(nullable: true),
                    Isdeleted = table.Column<bool>(nullable: false),
                    Isverify = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    ReceivedAmount = table.Column<decimal>(nullable: false),
                    FileUpload = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvoiceMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeadMaster",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(maxLength: 50, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    Phone = table.Column<string>(nullable: false),
                    Technologies = table.Column<int>(nullable: false),
                    Leadtype = table.Column<int>(nullable: false),
                    FileUpload = table.Column<string>(nullable: true),
                    Leaddate = table.Column<DateTime>(nullable: false),
                    Source = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    Url = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    OtherConnectivityPlatform = table.Column<int>(nullable: false),
                    ProfileName = table.Column<int>(nullable: false),
                    LeadCode = table.Column<string>(nullable: true),
                    IsLead = table.Column<bool>(nullable: false),
                    IsClose = table.Column<bool>(nullable: false),
                    IsProject = table.Column<bool>(nullable: false),
                    LeadStatus = table.Column<int>(nullable: false),
                    PrimaryConnectivityPlatform = table.Column<int>(nullable: false),
                    CloseReason = table.Column<string>(nullable: true),
                    CustomerId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeadMaster", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveBucket",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    Bucket = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveBucket", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveCount",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Userid = table.Column<string>(nullable: false),
                    Fromdate = table.Column<DateTime>(nullable: false),
                    Todate = table.Column<DateTime>(nullable: false),
                    Count = table.Column<string>(nullable: false),
                    EmployeeDescription = table.Column<string>(maxLength: 60, nullable: true),
                    HrDescription = table.Column<string>(maxLength: 60, nullable: true),
                    FileUpload = table.Column<string>(nullable: false),
                    Isapprove = table.Column<bool>(nullable: false),
                    IsReject = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    ApproveDate = table.Column<DateTime>(nullable: true),
                    Approveby = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    LeaveReason = table.Column<int>(nullable: false),
                    LeaveOtherReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveCount", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveHistory",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Userid = table.Column<string>(nullable: true),
                    Fromdate = table.Column<DateTime>(nullable: true),
                    Todate = table.Column<DateTime>(nullable: true),
                    Count = table.Column<string>(nullable: true),
                    EmployeeDescription = table.Column<string>(nullable: true),
                    HRDescription = table.Column<string>(nullable: true),
                    FileUpload = table.Column<string>(nullable: true),
                    Isapprove = table.Column<bool>(nullable: false),
                    IsDelete = table.Column<bool>(nullable: false),
                    ApproveDate = table.Column<DateTime>(nullable: true),
                    Approveby = table.Column<string>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    LeaveReason = table.Column<int>(nullable: false),
                    LeaveOtherReason = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveHistory", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LeaveManagement",
                columns: table => new
                {
                    LeaveManagementId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<Guid>(nullable: false),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    TotalLeaveDay = table.Column<decimal>(nullable: false),
                    LeaveBuket = table.Column<decimal>(nullable: false),
                    LeaveReason = table.Column<string>(nullable: true),
                    LeaveType = table.Column<int>(nullable: false),
                    LeaveDayType = table.Column<int>(nullable: false),
                    PaidLeaveCount = table.Column<decimal>(nullable: false),
                    UnPaidLeaveCount = table.Column<decimal>(nullable: false),
                    LeaveStatus = table.Column<int>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    UpdatedReasone = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: false),
                    UpdatedBy = table.Column<string>(nullable: true),
                    IsApproved = table.Column<bool>(nullable: false),
                    IsRejected = table.Column<bool>(nullable: false),
                    IsDeleted = table.Column<bool>(nullable: false),
                    UploadedFile = table.Column<string>(nullable: true),
                    IsMonthlyLeaveLeft = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeaveManagement", x => x.LeaveManagementId);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyReport",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Month = table.Column<string>(nullable: true),
                    EmployeeCode = table.Column<int>(nullable: true),
                    DataType = table.Column<string>(nullable: true),
                    _1 = table.Column<decimal>(name: "1", nullable: false),
                    _2 = table.Column<decimal>(name: "2", nullable: false),
                    _3 = table.Column<decimal>(name: "3", nullable: false),
                    _4 = table.Column<decimal>(name: "4", nullable: false),
                    _5 = table.Column<decimal>(name: "5", nullable: false),
                    _6 = table.Column<decimal>(name: "6", nullable: false),
                    _7 = table.Column<decimal>(name: "7", nullable: false),
                    _8 = table.Column<decimal>(name: "8", nullable: false),
                    _9 = table.Column<decimal>(name: "9", nullable: false),
                    _10 = table.Column<decimal>(name: "10", nullable: false),
                    _11 = table.Column<decimal>(name: "11", nullable: false),
                    _12 = table.Column<decimal>(name: "12", nullable: false),
                    _13 = table.Column<decimal>(name: "13", nullable: false),
                    _14 = table.Column<decimal>(name: "14", nullable: false),
                    _15 = table.Column<decimal>(name: "15", nullable: false),
                    _16 = table.Column<decimal>(name: "16", nullable: false),
                    _17 = table.Column<decimal>(name: "17", nullable: false),
                    _18 = table.Column<decimal>(name: "18", nullable: false),
                    _19 = table.Column<decimal>(name: "19", nullable: false),
                    _20 = table.Column<decimal>(name: "20", nullable: false),
                    _21 = table.Column<decimal>(name: "21", nullable: false),
                    _22 = table.Column<decimal>(name: "22", nullable: false),
                    _23 = table.Column<decimal>(name: "23", nullable: false),
                    _24 = table.Column<decimal>(name: "24", nullable: false),
                    _25 = table.Column<decimal>(name: "25", nullable: false),
                    _26 = table.Column<decimal>(name: "26", nullable: false),
                    _27 = table.Column<decimal>(name: "27", nullable: false),
                    _28 = table.Column<decimal>(name: "28", nullable: false),
                    _29 = table.Column<decimal>(name: "29", nullable: false),
                    _30 = table.Column<decimal>(name: "30", nullable: false),
                    _31 = table.Column<decimal>(name: "31", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyReport", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MonthlyTotalHours",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeCode = table.Column<int>(nullable: false),
                    Month = table.Column<string>(nullable: true),
                    TotalHours = table.Column<decimal>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MonthlyTotalHours", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotificationMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    NotifyAction = table.Column<string>(nullable: true),
                    NotifyController = table.Column<string>(nullable: true),
                    IsRead = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectActivity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectId = table.Column<int>(nullable: false),
                    UsersId = table.Column<string>(nullable: true),
                    Comment = table.Column<string>(nullable: true),
                    PenaltyDate = table.Column<DateTime>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    CreatedBy = table.Column<string>(nullable: true),
                    IsPenalty = table.Column<bool>(nullable: false),
                    IsAchievement = table.Column<bool>(nullable: false),
                    ConfigrationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMaster",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ProjectName = table.Column<string>(maxLength: 50, nullable: false),
                    Technologies = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 60, nullable: false),
                    ManagerName = table.Column<string>(nullable: false),
                    DeveloperName = table.Column<string>(nullable: false),
                    isactive = table.Column<bool>(nullable: false),
                    isdeleted = table.Column<bool>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreatedDate = table.Column<DateTime>(nullable: true),
                    UpdatedBy = table.Column<string>(nullable: true),
                    UpdatedDate = table.Column<DateTime>(nullable: true),
                    paymenttype = table.Column<string>(nullable: false),
                    projectamount = table.Column<string>(nullable: false),
                    currency = table.Column<int>(nullable: false),
                    StartedDate = table.Column<DateTime>(nullable: false),
                    ProjectStatus = table.Column<int>(nullable: false),
                    BankAccountId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMaster", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoleDetails",
                columns: table => new
                {
                    PageId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Pagename = table.Column<string>(nullable: false),
                    Rolename = table.Column<string>(nullable: true),
                    Isactive = table.Column<bool>(nullable: false),
                    Isdelete = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleDetails", x => x.PageId);
                });

            migrationBuilder.CreateTable(
                name: "TeamLeader",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TeamLeaderId = table.Column<string>(nullable: true),
                    TeamLeaderEmail = table.Column<string>(nullable: true),
                    EmployeeId = table.Column<string>(nullable: true),
                    EmployeeEmail = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TeamLeader", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Thoughts",
                columns: table => new
                {
                    ThoughtsID = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    thoughts = table.Column<string>(nullable: true),
                    Day = table.Column<string>(nullable: true),
                    Isactive = table.Column<bool>(nullable: false),
                    createdate = table.Column<DateTime>(nullable: false),
                    Updatedate = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thoughts", x => x.ThoughtsID);
                });

            migrationBuilder.CreateTable(
                name: "TimeSheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(nullable: true),
                    FromDate = table.Column<DateTime>(nullable: false),
                    ToDate = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeSheet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Todo",
                columns: table => new
                {
                    TodoId = table.Column<string>(nullable: false),
                    TodoItem = table.Column<string>(nullable: false),
                    Users = table.Column<string>(nullable: true),
                    Duedate = table.Column<DateTime>(nullable: false),
                    FileUpload = table.Column<string>(nullable: true),
                    IsDone = table.Column<bool>(nullable: false),
                    CreatedDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Todo", x => x.TodoId);
                });

            migrationBuilder.CreateTable(
                name: "weekdays",
                columns: table => new
                {
                    id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    dayname = table.Column<string>(nullable: true),
                    dayvalue = table.Column<string>(nullable: true),
                    isactive = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_weekdays", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(nullable: false),
                    ClaimType = table.Column<string>(nullable: true),
                    ClaimValue = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(nullable: false),
                    ProviderKey = table.Column<string>(nullable: false),
                    ProviderDisplayName = table.Column<string>(nullable: true),
                    UserId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    RoleId = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    LoginProvider = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");

            migrationBuilder.Sql("create PROCEDURE [dbo].[GetMemusByUserId]  --'0a61614e-6561-4c42-b2b0-e0f0fc3e681d,facf2307-4d16-450a-b10e-a7b1cb28f472','Dashboard','Index'    \r\n(      \r\n @RoleId nvarchar(max) =null,    \r\n @ControllerName nvarchar(max) =null,    \r\n @ActionName nvarchar(max) =null    \r\n)      \r\nAS      \r\nBEGIN  \r\n   \r\n if exists(Select 1 From Menus Where Method like '%' + @ActionName +'%' and Method like '%' + @ControllerName +'%')    \r\n BEGIN    \r\n  select top 1 m.Id     \r\n  from Role_Menu_Mapping ur    \r\n  inner join Menus m on ur.Menuid = m.Id    \r\n  where m.isactive = 1 and m.Controller like '%' + @ControllerName +'%'     \r\n  and m.Method like '%' + @ActionName +'%'    \r\n  and (ur.Roleid in ( select * from  [SDF_SplitString](@RoleId, ',')))       \r\n  group by m.Id     \r\n END    \r\n else    \r\n BEGIN    \r\n  select top 1  m.Id     \r\n  from Role_Menu_Mapping ur    \r\n  inner join Menus m on ur.Menuid =m.Id    \r\n  where m.isactive=1 and m.Controller like '%' + @ControllerName +'%'     \r\n  and (ur.Roleid in ( select * from  [SDF_SplitString](@RoleId, ',')))       \r\n  group by m.Id     \r\n END    \r\n  \r\nEND\r\nGO");
            migrationBuilder.Sql("Create procedure [dbo].[sp_GetBalanceSheetData]\r\n(\r\n\t@id\t\t\tint = 12\r\n)\r\nAS\r\nBEGIN\r\n\t\r\n\tDECLARE @RunTotalTestData TABLE  (\r\n\t\tId    int not null identity(1,1) primary key,\r\n\t\tDebit decimal null,\r\n\t\tCredit decimal null,\r\n\t\tDate\tDatetime,\r\n\t\tdescription nvarchar(MAX),\r\n\t\tBalanceType int,\r\n\t\tFileUpload\tnvarchar(MAX)\r\n\t);\r\n\r\n\tWith\tCTEBalanceData as(\r\n\t\tSelect\t\ta.Id as Id, \r\n\t\t\t\t\ta.CreatedDate as Date,\r\n\t\t\t\t\tb.projectname + ' (' + a.InvoiceNumber + ' - ' + CAST(a.Duedate as nvarchar(max)) + ')' as description,\r\n\t\t\t\t\t0 as debit,\r\n\t\t\t\t\ta.ReceivedAmount as credit,\r\n\t\t\t\t\t1 as balancetype,\r\n\t\t\t\t\ta.FileUpload,\r\n\t\t\t\t\tCAST(ISNULL(a.ReceivedAmount, 0) as decimal) as balance\r\n\t\tFROM\t\tInvoiceMaster as a\r\n\t\tINNER JOIN\tProjectMaster as b on a.ProjectId = b.Id\r\n\t\tINNER JOIN\tFinanceMaster as c on b.BankAccountId = c.Id\r\n\t\tWhere\t\tc.Id = @id\r\n\t\tUNION ALL\r\n\t\tSelect\t\ta.Id,\r\n\t\t\t\t\ta.expensedate,\r\n\t\t\t\t\ta.Description as description,\r\n\t\t\t\t\ta.Amount as debit,\r\n\t\t\t\t\t0 as credit,\r\n\t\t\t\t\t0 as balancetype,\r\n\t\t\t\t\ta.FileUpload,\r\n\t\t\t\t\tCAST(ISNULL(a.Amount, 0) as decimal) as balance\r\n\t\tFROM\t\tExpenseMaster as a\r\n\t\tINNER JOIN\tFinanceMaster as b on a.BankAccountId = b.Id\r\n\t\tWhere\t\tb.Id = @id\r\n\t)\r\n\tINSERT INTO @RunTotalTestData(Date, description, Debit, Credit, BalanceType, FileUpload)\r\n\tSELECT\t\tdate, description, debit, credit, balancetype, FileUpload\r\n\tFROM\t\tCTEBalanceData\r\n\tOrder by Date\r\n\r\n\t;WITH tempDebitCredit AS (\r\n\t\tSELECT\ta.id, \r\n\t\t\t\ta.debit, \r\n\t\t\t\ta.credit, \r\n\t\t\t\ta.Credit - a.Debit 'diff',\r\n\t\t\t\ta.Date,\r\n\t\t\t\ta.description,\r\n\t\t\t\ta.BalanceType,\r\n\t\t\t\ta.FileUpload\r\n\t\tFROM @RunTotalTestData a\r\n\t)\r\n\tSELECT a.id, a.Debit, a.Credit, CAST(SUM(b.diff) as decimal(18,2)) 'Balance',a.Date,\r\n\t\t\t\ta.description,\r\n\t\t\t\ta.BalanceType,\r\n\t\t\t\ta.FileUpload\r\n\tFROM   tempDebitCredit a,\r\n\t\t\ttempDebitCredit b\r\n\tWHERE b.id <= a.id\r\n\tGROUP BY a.id,a.Debit, a.Credit,a.Date,\r\n\t\t\t\ta.description,\r\n\t\t\t\ta.BalanceType,\r\n\t\t\t\ta.FileUpload\r\n \r\nEND\r\nGO ");
            migrationBuilder.Sql("Create PROCEDURE [dbo].[usp_BindGridDataRoleMenuMapping] \r\n \r\nAS  \r\nBEGIN  \r\n \r\nSELECT (Select top 1 name  from AspNetRoles where Id =a.Rolename) as rolename,\r\na.PageId , \r\na.Isactive,\r\n(\r\n SELECT STRING_AGG(Name, ',') \r\nFROM Menus where Id in (Select Item From dbo.SplitString(a.Pagename,','))) as menusname,\r\na.Pagename\r\nFROM RoleDetails  a\r\nWHERE  a.Isdelete <> 1 \r\n \r\nEND\r\nGO");
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[usp_GetEmailData] --5087\r\n(    \r\n@LeadId int = 0\r\n)    \r\nAS    \r\nBEGIN    \r\n  \r\n WITH LeadActivity_CTE AS\r\n(\r\n    select * from LeadActivity where IsEmail=1 and LeadId=@LeadId\r\n\t--union \r\n\t--select * from LeadActivity where isEmail=0 and LeadId=@LeadId\r\n)\r\nSELECT * FROM LeadActivity_CTE Order By CreatedDate Desc\r\n\r\nEND");
            migrationBuilder.Sql("create PROCEDURE [dbo].[usp_GetLeadActivityData] --@Ismaster = 1 , @IsTodayView = 1 , @Todaydate = '29-12-2022'\r\n(  \r\n@Ismaster  bit = 0,\r\n@UserId nvarchar(max) = null,\r\n@IsOverView bit = 0,\r\n@IsTodayView bit = 0,\r\n@IsNextView bit = 0,\r\n@IsWithoutView bit = 0,\r\n@IsthisView bit = 0,\r\n@Todaydate datetime = null\r\n)  \r\nAS  \r\nBEGIN  \r\n --declare @Ismaster  bit = 1,\r\n --@UserId nvarchar(max) = null\r\n\r\n\tselect \r\n\tLA.Id as Id,\r\n\tLA.LeadId as LeadId,\r\n\tLM.Name as LeadName,\r\n\tLA.Text as ActivityText,\r\n\tLA.TextDate as ActivityDate,\r\n\tLA.CreatedDate as CreatedDate,\r\n\tisnull(anu.UserName,'') as CreatedBy,\r\n\tLM.LeadCode  as LeadCode \r\n\tfrom LeadActivity as LA\r\n\tinner join LeadMaster as LM on LA.LeadId =  LM.Id\r\n\tLeft Outer join AspNetUsers as anu on LA.UserId = anu.Id\r\n\twhere LA.IsActivity = 1\r\n\tand LA.IsCompleted = 0\r\n\tand LM.IsLead = 1\r\n\tand ((@Ismaster = 1) OR ((@Ismaster = 0) AND (LA.UserId = @UserId)))\r\n\tand ((@IsOverView = 0) OR((@IsOverView = 1) AND (Cast(LA.TextDate as date) < Cast(@Todaydate as date))))\r\n\tand ((@IsTodayView = 0) OR((@IsTodayView = 1) AND (Cast(LA.TextDate as date) = Cast(@Todaydate as date))))\r\n\tand ((@IsNextView = 0) OR((@IsNextView = 1) AND (CAST(DATEADD(DAY, 8 - DATEPART(WEEKDAY, @Todaydate), CAST(@Todaydate AS DATE)) as date) < Cast(LA.TextDate as date))))\r\n\tand ((@IsthisView = 0) OR((@IsthisView = 1) \r\n     and ((CAST(DATEADD(DAY, 2 - DATEPART(WEEKDAY, @Todaydate), CAST(@Todaydate AS DATE)) as date) <= Cast(LA.TextDate as date)) \r\n and  (CAST(DATEADD(DAY, 8 - DATEPART(WEEKDAY, @Todaydate), CAST(@Todaydate AS DATE)) as date) >= Cast(LA.TextDate as date)))))\r\n\tand ((@IsWithoutView = 0) OR((@IsWithoutView = 1) AND (Cast(LA.TextDate as date) > Cast(@Todaydate as date))))\r\nEND\r\nGO");
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[usp_GetLeadActivityOnlyData] --3,9\r\n(    \r\n@LeadId int = 0,\r\n@LeadStatus int = 0\r\n)    \r\nAS    \r\nBEGIN    \r\n  \r\n select   \r\n LA.Id as Id,  \r\n LA.LeadId as LeadId,\r\n LA.Text as Text,  \r\n LA.TextDate as TextDate,\r\n LA.UserId as UserId,\r\n LA.LeadId\r\n\r\n as LeadId,\r\n LA.IsActivity as IsActivity,\r\n LA.IsComment as IsComment,\r\n LA.IsEmail as IsEmail,\r\n anu.UserName as UserName,\r\n LA.CreatedDate as CreatedDate,\r\n LA.IsCompleted as IsCompleted,\r\n LA.IsStatusChange as IsStatusChange,\r\n LA.FilePath,\r\n (Case when LA.OldStatusId > 0 then (select ISNULL(D.Text,'') from DataMaster as D where D.type =@LeadStatus and D.Id = LA.OldStatusId) else '' End) as OldStatusName,\r\n (Case when LA.NewStatusId > 0 then (select ISNULL(D.Text,'') from DataMaster as D where D.type =@LeadStatus and D.Id = LA.NewStatusId) else '' \r\nEnd) as NewStatusName,\r\n (select UserName from AspNetUsers where Id = tt.Manager) as Manager,\r\n (select UserName from AspNetUsers where Id = tt.Developer) as Developer,\r\n tt.FileUrl\r\n from LeadActivity as LA  \r\n Left Outer join AspNetUsers as anu on LA.UserId \r\n= anu.Id\r\n Left Outer join TestTaskMaster as tt on LA.LeadId = tt.LeadId\r\n where LA.LeadId = @LeadId\r\n\r\n order by LA.CreatedDate desc\r\nEND\r\n\r\n\r\n--select * from TestTaskMaster\r\nGO");
            migrationBuilder.Sql("Create PROCEDURE [dbo].[usp_GetLeadDataByStatus]\r\n(  \r\n@Ismaster  bit = 0,\r\n@UserId nvarchar(max) = null,\r\n@StatusId int = 0\r\n)  \r\nAS  \r\nBEGIN  \r\n\r\n\tselect \r\n\tLM.LeadCode as LeadCode,\r\n\tLM.id as Id,\r\n\tLM.Name as LeadName,\r\n\tLM.LeadStatus as LeadStatus,\r\n\t(select Text from DataMaster as DM where LM.Source = DM.Id) as Source, \r\n\t(select Text from DataMaster as DM where LM.Technologies = DM.Id) as Technologies,\r\n\tanu.UserName as CreatedBy,\r\n\tLM.CreatedDate as CreatedDate\r\n\tfrom LeadMaster as LM\r\n\tleft outer join AspNetUsers as anu on LM.CreatedBy = anu.Id\r\n\twhere LM.IsDelete = 0\r\n\tand LM.IsActive = 1\r\n\tand LM.IsLead = 1\r\n\tand ((@Ismaster = 1) OR ((@Ismaster = 0) AND (LM.CreatedBy = @UserId)))\r\n\tand LM.LeadStatus = @StatusId\r\nEND\r\nGO");
            migrationBuilder.Sql("create PROCEDURE [dbo].[usp_GetMenus] --1,0,0,0,0,'5b87344f-adeb-4237-a8e1-55bf27642946,a31900f8-e45e-409c-bccb-2f07d7575b3c'\r\n(  \r\n@Ismaster  bit = 0,  \r\n@IsParent  bit = 0,  \r\n@IsParentChildRoot  bit = 0,  \r\n@IsParentChild bit = 0,  \r\n@ParentId  int = 0,  \r\n@RoleIds  varchar(MAX)  \r\n)  \r\nAS  \r\nBEGIN  \r\n  \r\n IF(@Ismaster = 1)  \r\n BEGIN  \r\n  \r\n  SELECT b.Id,b.Name as PageName,b.Controller,b.Method,b.Ismaster,b.ParentID,b.URL,b.isactive,b.displayorder,LEN(b.Method),LEN(b.URL)   \r\n  From Role_Menu_Mapping as a  \r\n  INNER JOIN Menus as b On a.Menuid = b.Id  \r\n  Where b.isactive =1  \r\n   AND CAST(a.Roleid as varchar(max)) in (Select Item From dbo.SplitString(@RoleIds,','))  \r\n   AND b.Ismaster = 1  \r\n   AND b.ParentId = 0  \r\n   --AND (b.Method IS NULL OR LEN(b.Method) = 0)  \r\n   AND (b.URL IS NULL OR LEN(b.URL) = 0)  \r\n  \r\n END  \r\n ELSE IF(@IsParent = 1)  \r\n BEGIN  \r\n  \r\n  SELECT b.Id,b.Name as PageName,b.Controller,b.Method,b.Ismaster,b.Parentid,b.URL,b.isactive,b.displayorder   \r\n  From Role_Menu_Mapping as a  \r\n  INNER JOIN Menus as b On a.Menuid = b.Id  \r\n  Where b.isactive =1  \r\n   AND CAST(a.Roleid as varchar(max)) in (Select Item From dbo.SplitString(@RoleIds,','))  \r\n   AND (b.Ismaster = 1  \r\n   AND b.Parentid = @ParentId  \r\n   AND (b.Controller IS NULL OR LEN(b.Controller) = 0)  \r\n   AND (b.Method IS NULL OR LEN(b.Method) = 0)  \r\n   OR (b.Ismaster = 0  \r\n    AND b.ParentId = @ParentId))  \r\n  \r\n END  \r\n ELSE IF(@IsParentChildRoot = 1)  \r\n BEGIN  \r\n  \r\n  SELECT b.Id,b.Name,b.Controller,b.Method,b.Ismaster,b.Parentid,b.URL,b.isactive,b.displayorder   \r\n  From Role_Menu_Mapping as a  \r\n  INNER JOIN Menus as b On a.Menuid = b.Id  \r\n  Where b.isactive =1  \r\n   AND CAST(a.Roleid as varchar(max)) in (Select Item From dbo.SplitString(@RoleIds,','))  \r\n   AND b.Ismaster = 0  \r\n   OR (b.Ismaster = 0  \r\n    AND b.ParentId = @ParentId)  \r\n  \r\n END  \r\n ELSE IF(@IsParentChild = 1)  \r\n BEGIN  \r\n  \r\n  SELECT b.Id,b.Name,b.Controller,b.Method,b.Ismaster,b.Parentid,b.URL,b.isactive,b.displayorder   \r\n  From Role_Menu_Mapping as a  \r\n  INNER JOIN Menus as b On a.Menuid = b.Id  \r\n  Where b.isactive =1  \r\n   AND CAST(a.Roleid as varchar(max)) in (Select Item From dbo.SplitString(@RoleIds,','))  \r\n   AND b.Ismaster = 0  \r\n   AND b.ParentId = @ParentId  \r\n  \r\n END  \r\n  \r\nEND\r\nGO");
            migrationBuilder.Sql("CREATE PROCEDURE [dbo].[usp_GetTodoListData] --@Ismaster = 1\r\n(  \r\n@Ismaster  bit = 0,\r\n@UserId nvarchar(max) = null\r\n)  \r\nAS  \r\nBEGIN  \r\n--((@Ismaster = 1) OR ((@Ismaster = 0) AND (LM.CreatedBy = @UserId)))\r\n\r\n   select \r\n    TT.Id as Id,\r\n\tTT.AssignUsers as AssignUsers,\r\n\tTT.TodoItem as TodoItem,\r\n\tTT.Duedate as Duedate,\r\n\tTT.FileUpload as FileUpload,\r\n\tTT.Status as Status,\r\n\tTT.Priority as Priority,\r\n\tAnu.UserName as CreatedBy,\r\n\tTT.CreatedDate as CreatedDate\r\n\tfrom  TodoTask as TT\r\n\tLeft Outer Join AspNetUsers as Anu on TT.CreatedBy = Anu.Id\r\n\t--right outer join TodoTaskAssignUsers as ta on TT.Id = ta.TodoTaskId\r\n\twhere ((@Ismaster = 1) OR ((@Ismaster = 0) AND (TT.AssignUsers like '%' + @UserId + '%')))\r\n\r\n\torder by TT.Priority desc\r\n\t\r\nEND");
            migrationBuilder.Sql("create procedure [dbo].[usp_GetProjectList]  -- usp_GetProjectList 1,'2f315371-4a74-4e48-9a31-90ecbad4b8cf'  \r\n(  \r\n    @isadmin bit = 0,  \r\n    @userId nvarchar(MAX) = null  \r\n)  \r\nas   \r\nBEGIN  \r\n  \r\n   select   \r\n      p.Id,  \r\n   p.projectname,  \r\n   (select (FirstName +' '+ LastName) from AspNetUsers where id= p.managername) AS managername,  \r\n   (select (FirstName +' '+ LastName) from AspNetUsers where id= p.developername) AS developername,  \r\n   p.developername as developerId,  \r\n   p.[description],  \r\n   p.technologies,  \r\n   (select [Text] from Datamaster where id= p.technologies) AS TechnologyName,  \r\n   p.StartedDate,  \r\n   p.ProjectStatus,  \r\n   p.paymenttype,  \r\n   p.projectamount,  \r\n   (select [Text] from Datamaster where id = p.currency) AS CurrencyName,  \r\n   (select CONCAT(Accountownername, ' ',Acnumber, '', Bankname) from FinanceMaster where id = p.BankAccountId) AS BankAccontname,  \r\n   p.isactive,  \r\n   (select (FirstName +' '+ LastName) from AspNetUsers where id = p.createdby) AS createdby,  \r\n   (select (FirstName +' '+ LastName) from AspNetUsers where id = p.updatedby) AS updatedby,  \r\n   p.createddate,  \r\n   p.updateddate,  \r\n   Isnull((select Count(pa.Id) from ProjectActivity as pa where pa.ProjectId = p.Id and pa.IsPenalty = 1),0) as PenaltyCount,  \r\n   Isnull((select Count(pa.Id) from ProjectActivity as pa where pa.ProjectId = p.Id and pa.IsAchievement = 1),0) as AchievementCount  \r\n    FROM [dbo].[ProjectMaster] p WITH (NOLOCK)  \r\n where p.isdeleted = 0 AND (@isadmin = 1 OR (@isadmin = 0 AND (p.managername = @userId OR p.createdby = @userId OR p.developername = @userId)))  \r\n    order by p.createddate desc  \r\nEND");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "CandidateMaster");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Configuration");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "Credit");

            migrationBuilder.DropTable(
                name: "Datamaster");

            migrationBuilder.DropTable(
                name: "EmployeeHistory");

            migrationBuilder.DropTable(
                name: "ExpenseMaster");

            migrationBuilder.DropTable(
                name: "FinanceHistory");

            migrationBuilder.DropTable(
                name: "FinanceMaster");

            migrationBuilder.DropTable(
                name: "Hire");

            migrationBuilder.DropTable(
                name: "HolidayList");

            migrationBuilder.DropTable(
                name: "HRannouncement");

            migrationBuilder.DropTable(
                name: "InvoiceMaster");

            migrationBuilder.DropTable(
                name: "LeadMaster");

            migrationBuilder.DropTable(
                name: "LeaveBucket");

            migrationBuilder.DropTable(
                name: "LeaveCount");

            migrationBuilder.DropTable(
                name: "LeaveHistory");

            migrationBuilder.DropTable(
                name: "LeaveManagement");

            migrationBuilder.DropTable(
                name: "MonthlyReport");

            migrationBuilder.DropTable(
                name: "MonthlyTotalHours");

            migrationBuilder.DropTable(
                name: "NotificationMaster");

            migrationBuilder.DropTable(
                name: "ProjectActivity");

            migrationBuilder.DropTable(
                name: "ProjectMaster");

            migrationBuilder.DropTable(
                name: "RoleDetails");

            migrationBuilder.DropTable(
                name: "TeamLeader");

            migrationBuilder.DropTable(
                name: "Thoughts");

            migrationBuilder.DropTable(
                name: "TimeSheet");

            migrationBuilder.DropTable(
                name: "Todo");

            migrationBuilder.DropTable(
                name: "weekdays");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetMemusByUserId]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[sp_GetBalanceSheetData]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_BindGridDataRoleMenuMapping]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetEmailData]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetLeadActivityData]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetLeadActivityOnlyData]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetLeadDataByStatus]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetMenus]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetTodoListData]");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[usp_GetProjectList]");
        }
    }
}
