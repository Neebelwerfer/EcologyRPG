using System.Collections.Generic;

namespace EcologyRPG.Core.Character
{
    public class Characters
    {
        static Characters instance;

        public static float BaseMoveSpeed = 5f;
        public List<BaseCharacter> CharacterList = new();

        public static Characters Instance
        {
            get
            {
                instance ??= new Characters();
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