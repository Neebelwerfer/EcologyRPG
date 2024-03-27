using System.Collections.Generic;

namespace EcologyRPG.Core.Character
{
    public class CharacterManager
    {
        static CharacterManager instance;

        public List<BaseCharacter> CharacterList = new List<BaseCharacter>();

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

        public BaseCharacter GetCharacter(string name)
        {
            return CharacterList.Find(x => x.name == name);
        }

        public BaseCharacter GetCharacterByTag(string tag)
        {
            return CharacterList.Find(x => x.Tags.Contains(tag));
        }
        public BaseCharacter[] GetCharactersByTag(string tag)
        {
            return CharacterList.FindAll(x => x.Tags.Contains(tag)).ToArray();
        }
    }
}