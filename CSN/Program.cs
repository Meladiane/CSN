using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CSN
{
    public class Program
    {
        static void Main(string[] args)
        {

            string jsonFile = @"C:\Users\yc786\source\repos\CSN\file2.json";

            IStore store = new Store();

            store.Import(jsonFile);

            store.Buy("J.K Rowling - Goblet Of fire", "Isaac Asimov - Foundation",
             "J.K Rowling - Goblet Of fire");
        }
    }
}
