﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace MealPlan.Db.Model
{
    [Table("MealChange")]
    [Index(nameof(MealId), nameof(IngredientReferenceId), nameof(AddOrRemove), Name = "MealChange_UNIQUE_MealId_IngredientReferenceId_AddOrRemove", IsUnique = true)]
    public partial class MealChange
    {
        public MealChange()
        {
            InverseParentMealChange = new HashSet<MealChange>();
        }

        [Key]
        public int Id { get; set; }
        public int MealId { get; set; }
        public int IngredientReferenceId { get; set; }
        [Required]
        [StringLength(6)]
        [Unicode(false)]
        public string AddOrRemove { get; set; }
        [Column(TypeName = "decimal(18, 0)")]
        public decimal Amount { get; set; }
        public int? ParentMealChangeId { get; set; }

        [ForeignKey(nameof(IngredientReferenceId))]
        [InverseProperty("MealChanges")]
        public virtual IngredientReference IngredientReference { get; set; }
        [ForeignKey(nameof(MealId))]
        [InverseProperty("MealChanges")]
        public virtual Meal Meal { get; set; }
        [ForeignKey(nameof(ParentMealChangeId))]
        [InverseProperty(nameof(MealChange.InverseParentMealChange))]
        public virtual MealChange ParentMealChange { get; set; }
        [InverseProperty(nameof(MealChange.ParentMealChange))]
        public virtual ICollection<MealChange> InverseParentMealChange { get; set; }
    }
}