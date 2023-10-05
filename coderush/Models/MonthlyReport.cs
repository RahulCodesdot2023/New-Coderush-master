using System.ComponentModel.DataAnnotations.Schema;

namespace CodesDotHRMS.Models
{
    [Table("MonthlyReport")]
    public class MonthlyReport
    {
        public int Id { get; set; }
        public string Month { get; set; }
        public int? EmployeeCode { get; set; }
        public string DataType { get; set; }
        [Column("1")]
        public decimal _1 { get; set; }
        [Column("2")]
        public decimal _2 { get; set; }
        [Column("3")]
        public decimal _3 { get; set; }
        [Column("4")]
        public decimal _4 { get; set; }
        [Column("5")]
        public decimal _5 { get; set; }
        [Column("6")]
        public decimal _6 { get; set; }
        [Column("7")]
        public decimal _7 { get; set; }
        [Column("8")]
        public decimal _8 { get; set; }
        [Column("9")]
        public decimal _9 { get; set; }
        [Column("10")]
        public decimal _10 { get; set; }
        [Column("11")]
        public decimal _11 { get; set; }
        [Column("12")]
        public decimal _12 { get; set; }
        [Column("13")]
        public decimal _13 { get; set; }
        [Column("14")]
        public decimal _14 { get; set; }
        [Column("15")]
        public decimal _15 { get; set; }
        [Column("16")]
        public decimal _16 { get; set; }
        [Column("17")]
        public decimal _17 { get; set; }
        [Column("18")]
        public decimal _18 { get; set; }
        [Column("19")]
        public decimal _19 { get; set; }
        [Column("20")]
        public decimal _20 { get; set; }
        [Column("21")]
        public decimal _21 { get; set; }
        [Column("22")]
        public decimal _22 { get; set; }
        [Column("23")]
        public decimal _23 { get; set; }
        [Column("24")]
        public decimal _24 { get; set; }
        [Column("25")]
        public decimal _25 { get; set; }
        [Column("26")]
        public decimal _26 { get; set; }
        [Column("27")]
        public decimal _27 { get; set; }
        [Column("28")]
        public decimal _28 { get; set; }
        [Column("29")]
        public decimal _29 { get; set; }
        [Column("30")]
        public decimal _30 { get; set; }
        [Column("31")]
        public decimal _31 { get; set; }

    }
}
