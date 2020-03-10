using System.Linq;
using Cmd = DSharpPlus.CommandsNext.Command;
using Arg = DSharpPlus.CommandsNext.CommandArgument;
using FloatzelSharp.help;
using DSharpPlus.CommandsNext;

namespace FloatzelSharp.kekbot
{
    public static class CommandExtensions
    {

        public static Category GetCategory(this Cmd command) =>
            command?.CustomAttributes.OfType<CategoryAttribute>().FirstOrDefault()?.Category ?? Category.Unsorted;

        public static bool IsCustomRequired(this Arg argument) => argument?.CustomAttributes.OfType<RequiredAttribute>().Any() ?? false;

    }
}