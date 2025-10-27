namespace PoseidonPool.Domain.Entities.Cargo 
{
    public class CargoDetail : BaseEntity
    {
        public int CargoDetailId { get; set; }
        // gönderen müşteri
        public string SenderCustomer { get; set; }
        // alıcı müşteri 
        // mongoDb'den gelen id kullanılacağı için string türünde tanımlandı. Çünkü mongo db'de id string olarak tanımlanmıştı. Ayrıca MongoDb'de id'ler default olarak ObjectId türündedir.
        public string ReceiverCustomer { get; set; }
        public int Barcode { get; set; }
        // kargo firması
        public int CargoCompanyId { get; set; }
        // kargo firması property'si
        public CargoCompany CargoCompany { get; set; }
    }
}
