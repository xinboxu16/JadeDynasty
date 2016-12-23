using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace StorySystem
{
    public interface IStoryCommandFactory
    {
    }

    public class StoryCommandFactoryHelper<T> : IStoryCommandFactory where T : IStoryCommand, new()
    {
        
    }
}
