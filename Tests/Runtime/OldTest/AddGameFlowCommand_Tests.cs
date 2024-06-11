// using System.Collections;
// using GameFlow.Internal;
// using UnityEngine;
// using UnityEngine.TestTools;
// using UnityEngine.UI;
// using Assert = GameFlow.Internal.Assert;
//
// namespace GameFlow.Tests
// {
//     public class AddGameFlowCommand_Tests
//     {
//         public class TestScript___ElementAddPrefab : GameFlowElement
//         {
//         }
//
//         public class TestScript___ElementAddScene : GameFlowElement
//         {
//         }
//
//         public class TestScript___NoReference : GameFlowElement
//         {
//         }
//
//         private FadeLoading fadeLoading;
//         private ProgressLoading progressLoading;
//
//         [UnitySetUp]
//         public IEnumerator SetUp()
//         {
//             Builder.CreateMono<GameFlowRuntimeController>();
//             yield return null;
//             var loadingController = LoadingController.instance;
//             fadeLoading = loadingController.CreateChildMono<FadeLoading>()
//                 .AddCanvasGroup(0)
//                 .Disable();
//
//             progressLoading = loadingController.CreateChildMono<ProgressLoading>()
//                 .AddCanvasGroup(0)
//                 .CreateChildMono<ProgressLoading, Slider>((loading, slider) => loading.progressSlider = slider)
//                 .Disable();
//
//             loadingController.RegisterControllers(fadeLoading, progressLoading);
//             yield return null;
//         }
//
//         [UnityTest]
//         public IEnumerator _0_Single_Add_Execute_Command()
//         {
//             var next = false;
//             GameCommand.Add<TestScript___ElementAddPrefab>().OnCompleted(_ =>
//             {
//                 LoadingController.IsTransparentOn();
//                 next = true;
//             }).Build();
//             while (!next)
//             {
//                 yield return null;
//             }
//
//             var mono = PrefabTestMonoBehaviour.GetWithID("");
//             Assert.IsTrue(mono.onActiveCount == 1);
//             Assert.IsTrue(mono.onEnable);
//             GameFlowRuntimeController.CommandsIsEmpty();
//
//             var next2 = false;
//             GameCommand.Add<TestScript___ElementAddScene>().OnCompleted(_ =>
//             {
//                 LoadingController.IsTransparentOn();
//                 next2 = true;
//             }).Build();
//             while (!next2)
//             {
//                 yield return null;
//             }
//
//             yield return null;
//             var mono2 = SceneTestMonoBehaviour.GetWithID("");
//             Assert.IsTrue(mono2.onActiveCount == 1);
//             Assert.IsTrue(mono2.onEnable);
//             GameFlowRuntimeController.CommandsIsEmpty();
//             LoadingController.IsTransparentOff();
//         }
//
//         [UnityTest]
//         public IEnumerator _1_No_Reference_Add()
//         {
//             ErrorHandle.sendErrorIsLog = true;
//             var next = false;
//             GameCommand.Add<TestScript___NoReference>().OnCompleted(_ => { next = true; }).Build();
//             while (!next)
//             {
//                 yield return null;
//             }
//
//             yield return null;
//             GameFlowRuntimeController.CommandsIsEmpty();
//             ErrorHandle.sendErrorIsLog = false;
//             LoadingController.IsTransparentOff();
//         }
//
//         [UnityTest]
//         public IEnumerator _2_Double_Add_Execute_Command()
//         {
//             var next = false;
//             GameCommand.Add<TestScript___ElementAddPrefab>().Build();
//             GameCommand.Add<TestScript___ElementAddPrefab>().OnCompleted(_ => { next = true; }).Build();
//             while (!next)
//             {
//                 yield return null;
//             }
//
//             yield return null;
//             var mono = PrefabTestMonoBehaviour.GetWithID("");
//             Assert.IsTrue(mono.onCloseCount == 1, "PrefabTestMonoBehaviour.onCloseCount = " + mono.onCloseCount);
//             Assert.IsTrue(mono.onActiveCount == 2, "PrefabTestMonoBehaviour.onActiveCount = " + mono.onActiveCount);
//             Assert.IsTrue(mono.onEnable);
//             GameFlowRuntimeController.CommandsIsEmpty();
//
//             var next2 = false;
//             GameCommand.Add<TestScript___ElementAddScene>().Build();
//             GameCommand.Add<TestScript___ElementAddScene>().OnCompleted(_ => { next2 = true; }).Build();
//             while (!next2)
//             {
//                 yield return null;
//             }
//
//             yield return null;
//             var mono2 = SceneTestMonoBehaviour.GetWithID("");
//             Assert.IsTrue(mono2.onCloseCount == 1, "PrefabTestMonoBehaviour.onCloseCount = " + mono2.onCloseCount);
//             Assert.IsTrue(mono2.onActiveCount == 2, "PrefabTestMonoBehaviour.onActiveCount = " + mono2.onActiveCount);
//             Assert.IsTrue(mono.onEnable);
//             GameFlowRuntimeController.CommandsIsEmpty();
//         }
//
//         [UnityTest]
//         public IEnumerator _6_Single_Add_Release_Execute_Command()
//         {
//             yield return AddPrefab(1, 0, true, null);
//             yield return ReleasePrefab(1, 1, false, null);
//             yield return AddScene(1, 0, true, null);
//             yield return ReleaseScene(1, 1, false, null);
//             yield return null;
//             Assert.IsValidReference<TestScript___ElementAddScene>(false);
//             Assert.IsValidReference<TestScript___ElementAddPrefab>(false);
//         }
//
//         private static IEnumerator AddPrefab(int activeCount, int closeCount, bool enable, string id)
//         {
//             var next = false;
//             GameCommand.Add<TestScript___ElementAddPrefab>().OnCompleted(_ =>
//             {
//                 LoadingController.IsTransparentOn();
//                 next = true;
//             }).Build();
//
//             while (!next) yield return null;
//             yield return null;
//             var mono = PrefabTestMonoBehaviour.GetWithID(id);
//             Assert.IsTrue(mono.onActiveCount == activeCount);
//             Assert.IsTrue(mono.onCloseCount == closeCount);
//             Assert.IsTrue(mono.onEnable == enable);
//             GameFlowRuntimeController.CommandsIsEmpty();
//             LoadingController.IsTransparentOff();
//         }
//
//         private static IEnumerator ReleasePrefab(int activeCount, int closeCount, bool enable, string id)
//         {
//             var next = false;
//             GameCommand.Release<TestScript___ElementAddPrefab>().OnCompleted(_ =>
//             {
//                 var mono = PrefabTestMonoBehaviour.GetWithID(id);
//                 Assert.IsTrue(mono.onActiveCount == activeCount);
//                 Assert.IsTrue(mono.onCloseCount == closeCount);
//                 Assert.IsTrue(mono.onEnable == enable);
//                 LoadingController.IsTransparentOn();
//                 next = true;
//             }).Build();
//
//             while (!next) yield return null;
//             yield return null;
//             GameFlowRuntimeController.CommandsIsEmpty();
//             LoadingController.IsTransparentOff();
//         }
//
//         private static IEnumerator AddScene(int activeCount, int closeCount, bool enable, string id)
//         {
//             var next = false;
//             GameCommand.Add<TestScript___ElementAddScene>().OnCompleted(_ =>
//             {
//                 LoadingController.IsTransparentOn();
//                 next = true;
//             }).Build();
//
//             while (!next) yield return null;
//             yield return null;
//             var mono = SceneTestMonoBehaviour.GetWithID(id);
//             Assert.IsTrue(mono.onActiveCount == activeCount);
//             Assert.IsTrue(mono.onCloseCount == closeCount);
//             Assert.IsTrue(mono.onEnable == enable);
//             GameFlowRuntimeController.CommandsIsEmpty();
//             LoadingController.IsTransparentOff();
//         }
//         
//         private static IEnumerator ReleaseScene(int activeCount, int closeCount, bool enable, string id)
//         {
//             var next = false;
//             GameCommand.Release<TestScript___ElementAddScene>().OnCompleted(_ =>
//             {
//                 LoadingController.IsTransparentOn();
//                 next = true;
//             }).Build();
//
//             while (!next) yield return null;
//             yield return null;
//             GameFlowRuntimeController.CommandsIsEmpty();
//             LoadingController.IsTransparentOff();
//         }
//     }
// }