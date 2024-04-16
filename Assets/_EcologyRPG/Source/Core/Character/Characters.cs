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

        public void Clear()
        {
            CharacterList.Clear();
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

        public BaseCharacter GetCharacter(string guid)
        {
            var character = CharacterList.Find(x => x.GUID == guid);
            if (character == null)
            {
                UnityEngine.Debug.LogError($"Character with GUID: {guid} not found");
                return null;
            }
            return character;
        }
    }
}