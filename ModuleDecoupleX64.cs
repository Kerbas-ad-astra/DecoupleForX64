using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DecoupleForX64
{
    public class ModuleDecoupleX64 : ModuleDecouple 
    {
        AttachNode attachNode;
        Part cloupledPart;
        List<Part> cloupledParts;
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
                cloupledPart = base.part.parent;
                if (this.isOmniDecoupler)
                {
                    cloupledParts = new List<Part>(base.part.children.ToArray());
                }
                else
                {
                    if (this.attachNode.attachedPart != null)
                    {
                        cloupledPart = this.attachNode.attachedPart;
                    }
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
                    Vector3 direction = Vector3.zero;
                    Vector3 force = Vector3.zero;

                    if (this.isOmniDecoupler)
                    {
                        int num = cloupledParts.Count + 1;
                        foreach (Part current in cloupledParts)
                        {
                            direction = Vector3.Normalize(base.part.transform.position - current.transform.position);
                            force = direction * (-this.ejectionForce / (float)num);
                            Print("Force = " + force.magnitude);
                            current.Rigidbody.AddForce(force, ForceMode.Force);
                        }
                        if (cloupledPart != null)
                        {
                            direction = Vector3.Normalize(base.part.transform.position - cloupledPart.transform.position);
                            force = direction * (-this.ejectionForce / (float)num);
                            Print("Force = " + force.magnitude);
                            cloupledPart.Rigidbody.AddForce(direction * (-this.ejectionForce / (float)num), ForceMode.Force);
                        }
                    }
                    else
                    {
                        if (cloupledPart != null)
                        {
                            direction = Vector3.Normalize(base.part.transform.position - cloupledPart.transform.position);
                            force = direction * (this.ejectionForce * 0.5f);
                            base.part.Rigidbody.AddForce(force, ForceMode.Force);
                            cloupledPart.Rigidbody.AddForce(-force, ForceMode.Force);
                            Print("Force = " + force.magnitude);
                        }
                    }
                    didForce = true;
                }
            }
        }

        private static void Print(String s)
        {
            MonoBehaviour.print("[ModuleDecoupleX64] " + s);
        }


    }
}
