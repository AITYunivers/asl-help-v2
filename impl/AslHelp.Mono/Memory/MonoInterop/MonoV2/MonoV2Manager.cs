using System.Collections.Generic;

using AslHelp.Mono.Memory.Ipc;
using AslHelp.Mono.Memory.MonoInterop.MonoV1;

namespace AslHelp.Mono.Memory.MonoInterop.MonoV2;

internal class MonoV2Manager : MonoV1Manager
{
    public MonoV2Manager(IMonoMemoryManager memory)
        : base(memory, new MonoV2Initializer()) { }

    public override IEnumerable<nuint> GetFields(nuint klass)
    {
        while (klass != 0)
        {
            uint classKind = _memory.Read<uint>(klass + _structs["MonoClass"]["class_kind"]) & _structs["MonoClass"]["class_kind"];

            int fieldCount;
            nuint fields = _memory.Read<nuint>(klass + _structs["MonoClass"]["fields"]);

            if (classKind is 1 or 2)
            {
                fieldCount = _memory.Read<int>(klass + _structs["MonoClassDef"]["field_count"]);
            }
            else if (classKind == 3)
            {
                nuint genericInst = _memory.Read<nuint>(klass + _structs["MonoClassGenericInst"]["generic_class"]);
                nuint container = _memory.Read<nuint>(genericInst + _structs["MonoGenericClass"]["container_class"]);
                fieldCount = _memory.Read<int>(container + _structs["MonoClassDef"]["field_count"]);
            }
            else
            {
                fieldCount = 0;
            }

            for (int i = 0; i < fieldCount; i++)
            {
                nuint field = _memory.Read<nuint>(fields + (uint)(_structs["MonoClassField"].SelfAlignedSize * i));

                yield return field;
            }

            klass = _memory.Read<nuint>(klass + _structs["MonoClass"]["parent"]);
        }
    }
}
