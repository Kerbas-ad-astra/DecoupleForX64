using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecoupleForX64
{
    class ModuleAnchoredDecouplerX64 : ModuleAnchoredDecoupler
    {
        AttachNode attachNode;
        Part cloupledPart;
        bool didForce = false;
        int pFrameCount;

        public override void OnStart(PartModule.StartState state)
        {
            if (this.explosiveNodeID == "srf")
            {
                this.attachNode = base.part.srfAttachNode;
            }
            else
            {
                this.attachNode = base.part.findAttachNode(this.explosiveNodeID);
            }
            if (this.attachNode == null)
            {
                Print("Error: No attachnode found with id " + this.explosiveNodeID);
            }
            didForce = this.isDecoupled;
            base.OnStart(state);
        }

        public void FixedUpdate()
        {
            if (!this.isDecoupled)
            {
                if (this.attachNode.attachedPart != null && !(this.attachNode.attachedPart == null))
                {
                    cloupledPart = this.attachNode.attachedPart;
                }
                pFrameCount = 1;
            }
            else if (!didForce)
            {
                if (pFrameCount != 0)
                {
                    //Print("Waiting for 1 more physic frame");
                    pFrameCount--;
                }
                else
                {
                    if (cloupledPart != null)
                    {
                        float force = this.ejectionForce * 0.5f;
                        Print("Force = " + force);
                        base.part.rigidbody.AddForceAtPosition(base.part.transform.right * -force, base.part.transform.position, ForceMode.Force);
                        cloupledPart.Rigidbody.AddForceAtPosition(base.transform.right * force, base.transform.position, ForceMode.Force);
                    }

                    didForce = true;
                }
            }
        }

        private static void Print(String s)
        {
            MonoBehaviour.print("[ModuleAnchoredDecouplerX64] " + s);
        }


    }
}
