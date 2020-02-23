using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SUHScripts.Functional;
using static SUHScripts.Functional.Functional;
using SUHScripts.Tests;

namespace SUHScripts.Functional.Tests
{ 
    public class OptionTest : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TestValidator<int> intsEqual = (a, b) => a == b;
            Option<int> oneOpt = 1.AsOption();
            Option<int> twoOpt = 2.AsOption();

            Option<int> intNone = NONE;

            //Reduce from value when some 
            var reduceValueResult0 =
                oneOpt
                .Bind(val => (val + 4).AsOption()) // 1 + 4 = 5
                .Reduce(-1); // 4

            reduceValueResult0.Validate(5, intsEqual, "Reduce from value when some");

            //Reduce from value when None
            var reduceValueResult1 =
                oneOpt
                .Bind(val => (val + 4).AsOption())
                .Bind(val => (Option<int>)NONE)
                .Reduce(-1);

            reduceValueResult1.Validate(-1, intsEqual, "Reduce from value when none");

            //Reduce from function when some
            var reduceFunctionResult0 =
                oneOpt
                .Bind(val => (val + 4).AsOption()) // 1 + 4 = 4
                .Reduce(() => -1); // 5

            reduceFunctionResult0.Validate(5, intsEqual, "Reduce from function when some");

            //Reduce from function when None
            var reduceFunctionResult1 =
                oneOpt
                .Bind(val => (val + 4).AsOption())
                .Bind(val => (Option<int>)NONE)
                .Reduce(() => -1);

            reduceFunctionResult1.Validate(-1, intsEqual, "Reduce from function when none");

            //Map when some
            var mapResult0 =
            oneOpt
                .Map(val => val + 1) // 1 + 1 = 2
                .Map(val => val * 2) // 2 * 2 = 4
                .Reduce(-1); // Reduce to 4

            mapResult0.Validate(4, intsEqual, "Map when some");

            //Map when none
            var mapResult1 =
            intNone
                .Map(x => x + 1) // NONE
                .Map(val => val * 2) //NONE
                .Reduce(-1); // Reduce to -1

            mapResult1.Validate(-1, intsEqual, "Map when none");

            //Bind when some
            var bindResult0 =
                oneOpt
                .Bind(outer => twoOpt.Bind(inner => (Option<int>) (outer + inner)))
                .Reduce(-1);

            bindResult0.Validate(3, intsEqual, "Bind when some");

            //Bend when None
            var bindResult1 =
                oneOpt
                .Bind(outer => intNone.Bind(inner => (Option<int>)(outer + inner)))
                .Reduce(-1);

            bindResult1.Validate(-1, intsEqual, "Bind when none");

            //Where compares to none
            var whereResult0 =
                oneOpt
                .Bind(val => (val * 3).AsOption()) // 1 * 3 = 3
                .Where(val => val % 2 == 0) // 3 % 2 = 1 -> NONE
                .Reduce(-1); //Reduced to -1

            whereResult0.Validate(-1, intsEqual, "Where compares to none");

            //Where compares to some
            var whereResult1 =
                oneOpt
                .Bind(val => (val * 4).AsOption()) // 1 * 4 = 4
                .Where(val => val % 2 == 0) // 4 % 2 = 0 -> true
                .Reduce(-1); // 4

            whereResult1.Validate(4, intsEqual, "Where compares to some");
        }
        
    }
}

