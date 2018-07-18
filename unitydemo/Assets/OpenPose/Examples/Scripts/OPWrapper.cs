﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace opdemo.examples
{
    using opdemo.dll;

    public class OPWrapper : OP_API
    {
        protected override void OPOutput(string message, int type = 0)
        {
            base.OPOutput(message, type);

            switch (type)
            {
                case 0:
                    OutputController.instance.PushNewOutput(message);
                    break;
                case 1:
                    OutputController.instance.PushNewImage(message);
                    break;
            }
        }

        protected override void Awake()
        {
            base.Awake();

            OPSetParameter(OPFlag.HAND);
            OPRun();
        }

        // Use this for initialization
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
