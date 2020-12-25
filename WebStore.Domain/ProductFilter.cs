namespace WebStore.Domain
{
    public class ProductFilter
    {
        public int? SectionId { get; set; }

        public int? BrandId { get; set; }

        /// <summary>Список идентификаторов товаров, извлекаемых с помощью фильтра</summary>
        public int[] Ids { get; set; }
    }
}
