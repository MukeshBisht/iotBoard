

using System;
using System.Collections.Generic;

namespace IOT.BL {

    public static class ActionQueue  {
        public static Queue<int> queue = new Queue<int>();
        public static int lastTask = 0;

        public static int NextAction(){
            if(queue.Count > 0){
                return queue.Dequeue();
            }

            return 0;
        }

        public static void AddAction(int action){
            if(!queue.Contains(action))
                queue.Enqueue(action);
        }
    }
}