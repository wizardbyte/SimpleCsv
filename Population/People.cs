using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Population
{
    public class People
    {
        private List<Person> _people = new List<Person>();

        public People()
        {
            _people.Add(new Person() { firstName = "Philip", middleName = "J.", lastName = "Fry", quote = "Space. It seems to go on forever. But then you get to the end and the gorilla starts throwin' barrels at you." });
            _people.Add(new Person() { firstName = "Turanga", middleName = "", lastName = "Leela", quote = "Oh, if only we hadn't flown penguins to Pluto and dumped oil on them, this might never have happened." });
            _people.Add(new Person() { firstName = "Bender", middleName = "Bending", lastName = "Rodriguez", quote = "Bite my shiny metal ass." });
            _people.Add(new Person() { firstName = "Hubert", middleName = "J.", lastName = "Farnsworth", quote = "Good news, everyone." });
            _people.Add(new Person() { firstName = "John", middleName = "", lastName = "Zoidberg", quote = "Now Fry, it's been a few years since medical school, so remind me. Disemboweling in your species: fatal or non-fatal?" });
            _people.Add(new Person() { firstName = "Amy", middleName = "", lastName = "Wong", quote = "Psst, look what life was like before genetic engineering." });
            _people.Add(new Person() { firstName = "Hermes", middleName = "", lastName = "Conrad", quote = "Don't be a hero, Fry! It's not covered in the health plan!" });
            _people.Add(new Person() { firstName = "Zap", middleName = "", lastName = "Brannigan", quote = "Kif, I'm sensing a very sensual disturbance in the force. Prepare for ship-to-ship intimacy." });
            _people.Add(new Person() { firstName = "Kif", middleName = "", lastName = "Kroker", quote = "I'll get the powder, sir." });
            _people.Add(new Person() { firstName = "Lord", middleName = "", lastName = "Nibbler", quote = "Everyone, out of the universe!" });
            _people.Add(new Person() { firstName = "LeRoy", middleName = "Test", lastName = "Person", quote = "This is a string \"with quotes\", a couple commas, and some additional text.", birthdate = new DateTime(1959, 06, 23), favouriteColour = Color.BlueViolet });
            
        }

        public IQueryable<Person> QueryThePeople
        {
            get
            {
                return _people.AsQueryable<Person>();
            }
        }
    }
}
