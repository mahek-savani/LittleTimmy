// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.AI;

// public class State_Machine_Parent
// {
//     public NavMeshAgent agent;
//     private abstract class State
//     {
//         protected enum STATE {IDLE, PATROLLING, CHASING};
//         protected enum PHASE {START, UPDATE, STOP}
//         protected STATE name;
//         protected PHASE phase;
//         public abstract void start();
//         public abstract void loop();
//         public abstract void stop();
//         public virtual void process()
//         // {
//         //     switch (phase){
//         //         case PHASE.START:
//         //             start();
//         //             break;
//         //         case PHASE.UPDATE:
//         //             loop();
//         //             break;
//         //         case PHASE.STOP:
//         //             stop();
//         //             break;
//         //     }
//         // }
//     }

//     private class Idle_State : State
//     {
//         public Idle_State()
//         {
//             name = STATE.IDLE;
//             phase = PHASE.START;
//         }
//         public override void start()
//         {
//             phase = PHASE.UPDATE;
//         }

//         public override void loop()
//         {
//             agent.SetDestination();
//         }

//         public override void stop()
//         {
//             phase = PHASE.STOP;
//         }
//     }

//     private class Patrol_State : State
//     {
//         private List<Vector3> wayPoints;
//         public Patrol_State(in List<Vector3> _wayPoints)
//         {
//             name = STATE.PATROLLING;
//             phase = PHASE.START;
//             wayPoints = _wayPoints;
//         }
//         public override void start()
//         {
//             phase = PHASE.UPDATE;
//         }

//         public override void loop()
//         {
            
//         }

//         public override void stop()
//         {
//             phase = PHASE.STOP;
//         }
//     }
// }
