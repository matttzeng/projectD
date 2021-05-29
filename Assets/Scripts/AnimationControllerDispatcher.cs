using UnityEngine;

namespace ProjectDInternal
{
    /// <summary>
    /// Need to be added to the GameObject that have the AnimatorController. This will receive the Event defined in the
    /// import of the animations and can dispatch them to some receivers. Used by step event and attack frame event on
    /// characters.
    /// </summary>
    public class AnimationControllerDispatcher : MonoBehaviour
    {
        public interface IAttackFrameReceiver
        {
            void AttackFrame();
          
        }

        public interface IFootstepFrameReceiver
        {
            void FootstepFrame();
        }

        public interface ISkillFrameReceiver
        {
            void SkillFrame();
        }


        public MonoBehaviour AttackFrameReceiver;
        public MonoBehaviour FootstepFrameReceiver;
        public MonoBehaviour SkillFrameReceiver;

        IAttackFrameReceiver m_AttackReceiver;
        IFootstepFrameReceiver m_FootstepFrameReceiver;
        ISkillFrameReceiver m_SkillFrameReceiver;

        void Awake()
        {
            if (AttackFrameReceiver != null)
            {
                m_AttackReceiver = AttackFrameReceiver as IAttackFrameReceiver;

                if (m_AttackReceiver == null)
                {
                    Debug.LogError("The Monobehaviour set as Attack Frame Receiver don't implement the IAttackFrameReceiver interface!", AttackFrameReceiver);
                }
            }

            if (FootstepFrameReceiver)
            {
                m_FootstepFrameReceiver = FootstepFrameReceiver as IFootstepFrameReceiver;
            
                if (m_AttackReceiver == null)
                {
                    Debug.LogError("The Monobehaviour set as Footstep Frame Receiver don't implement the IFootstepFrameReceiver interface!", FootstepFrameReceiver);
                }
            }
            if (SkillFrameReceiver)
            {
                m_SkillFrameReceiver = SkillFrameReceiver as ISkillFrameReceiver;

                if (m_AttackReceiver == null)
                {
                    Debug.LogError("The Monobehaviour set as Skill Frame Receiver don't implement the ISkillFrameReceiver interface!", SkillFrameReceiver);
                }
            }
        }

        void AttackEvent()
        {
            m_AttackReceiver?.AttackFrame();
        }

        void FootstepEvent()
        {
            m_FootstepFrameReceiver?.FootstepFrame();
        }

        void SkillEvent()
        {
            m_SkillFrameReceiver?.SkillFrame();
        }
    }
}