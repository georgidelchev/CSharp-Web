-- RECIPES QUERIES
select count(id) as RecipesCount from recipes
select count(id) as RecipeIngredientsCount from recipeingredients
select count(id) as IngredientsCount from ingredients
select count(id) as ImagesCount from images
select count(id) as CategoriesCount from categories

select * from recipes
select * from recipeingredients
select * from ingredients
select * from images
select * from categories

-- Check for retarded fields
select * from ingredients where name like ('[А-За-з]%') and UNICODE(substring(name,1,1)) <> UNICODE(lower(substring(name,1,1)))
select * from ingredients where name like ('за %') and UNICODE(substring(name,1,1)) = UNICODE(lower(substring(name,1,1)))
select * from ingredients where name like (' %')
select * from ingredients where name like ('[0-9]%')



