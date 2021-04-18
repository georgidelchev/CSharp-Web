﻿using MyRecipes.Data.Common.Models;

namespace MyRecipes.Data.Models
{
    public class Vote : BaseModel<int>
    {
        public int RecipeId { get; set; }

        public virtual Recipe Recipe { get; set; }

        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public byte Value { get; set; }
    }
}
