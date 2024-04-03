using System.Collections.Generic;

namespace EcologyRPG._Core.Character
{
    public class CharacterManager
    {
        static CharacterManager instance;

        public List<BaseCharacter> CharacterList = new();

        public static CharacterManager Instance
        {
            get
            {
                instance ??= new CharacterManager();
                return instance;
            }
        }

        public void AddCharacter(BaseCharacter character)
        {
            CharacterList.Add(character);
        }

        public void RemoveCharacter(BaseCharacter character)
        {
            CharacterList.Remove(character);
        }

        public BaseCharacter[] GetCharactersByTag(string tag)
        {
            return CharacterList.FindAll(x => x.Tags.Contains(tag)).ToArray();
        }
    }
}