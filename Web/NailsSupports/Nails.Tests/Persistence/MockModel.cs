namespace NailsFramework.Tests.Persistence
{
    public class MockModel
    {
        public MockModel(int id)
        {
            Id = id;
        }

        public MockModel()
        {
        }

        public int Id { get; set; }
    }
}