using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CodesDotHRMS.Migrations
{
    public partial class CreateManaualAttTime : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManualTimeSheet",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<string>(nullable: true),
                    ManualDate = table.Column<DateTime>(nullable: false),
                    FromTime = table.Column<DateTime>(nullable: false),
                    ToTime = table.Column<DateTime>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    IsApprove = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTimeSheet", x => x.Id);
                });
            migrationBuilder.Sql("CREATE PROCEDURE GetEmployeeMonthlyReport\r\n    @EmployeeCode INT,\r\n    @Day INT,\r\n\t@Month nvarchar(Max)\r\nAS\r\nBEGIN\r\n    DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)\r\n\r\n    -- Create the dynamic SQL query for PIVOT\r\n    SET @DynamicPivotQuery = N'\r\n    SELECT ' + CAST(@Day AS NVARCHAR(2)) + ' AS [Day],[Month], EmployeeCode, [ArrTim], [Out1], [In2], [Out2], [In3],[Out3],[In4],[Out4],[In5],[Out5],[In6],[Out6],[In7],[DepTim], [WorkingHrs], [ReportType]\r\n    FROM (\r\n        SELECT EmployeeCode,[Month], DataType, [' + CAST(@Day AS NVARCHAR(2)) + '] AS ColValue\r\n        FROM MonthlyReport\r\n        WHERE EmployeeCode = ' + CAST(@EmployeeCode AS NVARCHAR(10)) + ' and [Month]=''' + @Month + '''\r\n    ) AS SourceTable\r\n    PIVOT (\r\n        MAX(ColValue)\r\n        FOR DataType IN ([ArrTim], [Out1], [In2], [Out2], [In3],[Out3],[In4],[Out4],[In5],[Out5],[In6],[Out6],[In7],[DepTim], [WorkingHrs], [ReportType])\r\n    ) AS PivotTable;'\r\n\r\n    -- Execute the dynamic SQL query\r\n    EXEC sp_executesql @DynamicPivotQuery\r\nEND");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManualTimeSheet");
            migrationBuilder.Sql("DROP PROCEDURE [dbo].[GetEmployeeMonthlyReport]");
        }
    }
}
