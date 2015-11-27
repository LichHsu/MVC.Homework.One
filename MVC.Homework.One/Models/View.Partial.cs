namespace MVC.Homework.One.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    [MetadataType(typeof(ViewMetaData))]
    public partial class View
    {
    }
    
    public partial class ViewMetaData
    {
        [Required]
        public int Id { get; set; }
        
        [StringLength(50, ErrorMessage="欄位長度不得大於 50 個字元")]
        [Required]
        public string 客戶名稱 { get; set; }
        public Nullable<int> 聯絡人總數 { get; set; }
        public Nullable<int> 帳戶總數 { get; set; }
    }
}
