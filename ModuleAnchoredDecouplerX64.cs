using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DecoupleForX64
{
    class ModuleAnchoredDecouplerX64 : ModuleAnchoredDecoupler
    {
        bool doForce = false;

        [KSPAction("Decouple")]
        public new void DecoupleAction(KSPActionParam param)
        {
            doForce = true;
        }

        [KSPEvent(guiName = "Decouple", guiActive = true)]
        public new void Decouple()
        {
            doForce = true;
        }

        public new void OnDecouple()
        {
            doForce = true;
        }

        public override void OnActive()
        {
            if (base.staged)
            {
                doForce = true;
            }
        }

        public void FixedUpdate()
        {
            if (!this.isDecoupled && doForce)
            {
                print("[ModuleAnchoredDecouplerX64] ejectionForce " + this.ejectionForce);

                this.ejectionForce = this.ejectionForce * 100;
                base.OnDecouple();
            }
            doForce = false;
        }
    }
}
