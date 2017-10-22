using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Problend.Models
{
    public class Product
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Артикулен номер")]
        [Range(100, 999, ErrorMessage = "Артикулния номер трябва да е има стойност между 100 и 999")]
        public int ProductNumber { get; set; }

        [Required]
        [Display(Name = "Вид продукт")]
        public string ProductType { get; set; }

        [Required]
        [Display(Name = "Вид марка")]
        public string ProductBrand { get; set; }

        public int ClientId { get; set; }
        public virtual Client Client { get; set; }

        [Display(Name = "Вид разфасовка")]
        public string TypeOfUnit { get; set; }

        [Display(Name = "Брой на разфасовката")]
        public int NumberOfUnits { get; set; }

        [Required]
        [Display(Name = "Вид на пакетчето")]
        public string TypeOfPackage { get; set; }

        [Display(Name = "Брой пакетчета в разфасовка")]
        public int NumberOfItemsInPackage { get; set; }

        [Required]
        [Display(Name = "Вложен грамаж в пакетче")]
        public double WeightInputedInPackage { get; set; }

        [Required]
        [Display(Name = "Реален грамаж на пакетче")]
        public double RealPackageWeight { get; set; }

        [Display(Name = "Вид на хартията")]
        public string PaperType { get; set; }

        [Display(Name = "Размер на пакетчето")]
        public string PackageSize { get; set; }

        public int CartonBoxID { get; set; }
        public virtual CartonBox CartonBox { get; set; }

        [Display(Name = "Грамаж претеглена опаковка")]
        public double MeasuredPackagingWeight { get; set; }

        [Display(Name = "Брой стикове претеглена опаковка")]
        public double NumberOfSticksMeasured { get; set; }

        [Display(Name = "Нето на разфасовката без продукт")]
        public double GrossWeightUnit { get; set; }

        [Display(Name = "Брой транспортни единици на европалет")]
        public int PiecesOnEuropallet { get; set; }

        [Display(Name = "Снимка палетизация")]
        public string PalletizationPicturePath { get; set; }

        [Display(Name = "Снимка на транспортната единица")]
        public string TransportationUnitPicturePath { get; set; }

        [Display(Name = "Снимка на етикета на разфасовката")]
        public string TransportationUnitLabelPicturePath { get; set; }

        [Display(Name = "Снимка на етикета на транспортната единица")]
        public string UnitLabelPicturePath { get; set; }

        [Display(Name ="Снимка на пакетчето")]
        public string PackagePicturePath { get; set; }

        [Display(Name = "Снимка на кутията предна")]
        public string DisplayBoxPictureFront { get; set; }

        [Display(Name = "Снимка на кутията задна")]
        public string DisplayBoxPictureBack { get; set; }

        [Display(Name = "Опаковка за разфасовка (кутия/торба) в кг.")]
        public double PackagingUsedForUnit { get; set; }

        [Display(Name = "Опаковка за 1000 бр. в кг.")]
        public double PackagingUsedFor1000Pieces { get; set; }

        [Display(Name = "Суровина в кг. вложена в един брой разфасовка(кутия/торба)")]
        public double RawMaterialUsedInUnit { get; set; }

        [Display(Name = "Бруто тегло за разфасовка (кутия/торба в кг.)")]
        public double GrossWeightOfUnit { get; set; }

        [Display(Name = "Бруто тегло на транспортна единица")]
        public double GrossWeightCartonBox { get; set; }

        public IEnumerable<SelectListItem> ProductTypeList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Моля изберете...", Value = "" },
                    new SelectListItem { Text = "Бяла захар", Value = "Бяла захар" },
                    new SelectListItem { Text = "Кафява захар", Value = "Кафява захар" },
                    new SelectListItem { Text = "Мед", Value = "Мед" },
                    new SelectListItem { Text = "Сметана", Value = "Сметана" },
                    new SelectListItem { Text = "Бисквита", Value = "Бисквита" },
                    new SelectListItem { Text = "Подсладител", Value = "Подсладител" },
                    new SelectListItem { Text = "Стевия", Value = "Стевия" },
                };
            }
        }
       
        public IEnumerable<SelectListItem> TypeOfPackageList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Моля изберете...", Value = "" },
                    new SelectListItem { Text = "Стик", Value = "Стик" },
                    new SelectListItem { Text = "Саше", Value = "Саше" },
                    new SelectListItem { Text = "Стендпак", Value = "Стендпак" },
                };
            }
        }

        public IEnumerable<SelectListItem> PaperTypeList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Моля изберете...", Value = "" },
                    new SelectListItem { Text = "Хромова", Value = "Хромова" },
                    new SelectListItem { Text = "Кафява рипсова", Value = "Кафява рипсова" },
                    new SelectListItem { Text = "Двуслойна (PP + LDPE)", Value = "Двуслойна (PP + LDPE)" },
                    new SelectListItem { Text = "PAP/AL/LDPE", Value = "PAP/AL/LDPE" },
                    new SelectListItem { Text = "PET + LDPE", Value = "PET + LDPE" },
                    new SelectListItem { Text = "PP+PP", Value = "PP+PP" },
                };
            }
        }

        public IEnumerable<SelectListItem> TypeOfUnitList
        {
            get
            {
                return new List<SelectListItem>
                {
                    new SelectListItem { Text = "Моля изберете...", Value = "" },
                    new SelectListItem { Text = "Торба", Value = "Торба" },
                    new SelectListItem { Text = "Кутия", Value = "Кутия" },
                    new SelectListItem { Text = "Стендпак", Value = "Стендпак" },
                    new SelectListItem { Text = "Насипно", Value = "Насипно" },
                };
            }
        }
    }

  
}