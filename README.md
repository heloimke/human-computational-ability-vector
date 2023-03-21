# **H**uman **C**omputational **A**bility **V**ector

## What is the **HCAV**?
**An attempt at standardising a quantitative fitness vector for abstract cognition ability.**<br />
Essentially a multidimensional replacement for IQ that assumes only extremely rudementary recollection requiements, such as basic familiarity with using a computer, and reasonable familiarity with language and logic.<br />
As this project grows it's likely the vector will change frequently, as such no formal static definitions, such as dimension content or L2norm exist. But here is a non exhaustive list of sections of the vector already outlined:
 - Abstract Working Memory
   - Normalised Concurrency
   - Normalised Unit Specificity
   - Normalised Unit Complexity
 - Auditory Working Memory
   - Normalised Concurrency
   - Normalised Unit Specificity
   - Normalised Unit Complexity
 - Visual Working Memory
   - Normalised Concurrency
   - Normalised Unit Specificity
   - Normalised Unit Complexity
 - Latent Abstract Recall
   - Normalised Latency
   - Normalised Concurrency
 - Latent Spacial Recall (Visual + Auditory + Abstract)
   - Normalised Latency
   - Normalised Complexity
   - Normalised Accuracy
 - Latent Sequence Recall (Visual + Auditory + Abstract)
   - Normalised Latency
   - Normalised Sequential Depth
 - Concurrent Sequential Delayed Recall (Visual + Auditory + Abstract)
   - Normalised Maximum Depth
   - Normalised (Depth + Accuracy vs. Time) correlations
 - Sequential Set Accumulation Sensitivity (Visual + Auditory + Abstract)
   - Scalar Normalisation of \[Complexity vs. Accuracy\] correlations
   - Scalar Normalisation of \[Speed vs. Accuracy\] correlations
 - Singlet Reaction Time (Visual + Cymatic)
 - Processing Speed
   - Scalar Normalisation of \[Steps vs. Speed\] correlations
   - Scalar Normalisation of \[Accuracy vs. Steps + Speed\] correlations
 - LUT Processing Ability
   - Normalised LUT Complexity
   - Normalised Processing Depth
   - Normalised Accuracy
 - Arithmetic Ability
   - Scalar Normalisation of \[Complexity vs. Speed\] correlations
   - Scalar Normalisation of \[Accuracy vs. Complexity + Speed\] correlations
 - Correlative Ability (Visual + Auditory + Spatial + Abstract)
   - Scalar Normalisation of \[Complexity vs. Speed\] correlations
   - Scalar Normalisation of \[Accuracy vs. Complexity + Speed\] correlations
 - Theoretical Ability
   - Normalised Axiomatic Depth
   - Normalised Axiomatic Complexity
   - Scalar Normalisation of \[Axiom Complexity * Depth vs. Accuracy + Speed\]
   
*Key: + is concatenate and \* is dot-product or multiplication*
   
The goal is to create a plethora of various tests that aim to deliniate a players ability in at least one dimension, as well as develop subsets that predict an individuals performance on tests excluded from the subset as accurately as possible.<br />
Assuming an accurate subset is found it can be used as a standardised examination routine that maps interactions with this application to a vector. This vector can then go on to be used in research aimed at analysing an individuals performance over various changes, such as; pharmacological trials, brainwave entrainment, physical and mental exersize regiments, or emotional and mental health.

## What is this repository?
**The official desktop client for recording and analysing human performance on the standard HCAV tests.**<br />
This application is a complete functional solution for research, examination, and training for HCAV performance.
Hopefully, regardless of the HCAV metric's popularity or future works extending the HCAV standard, this application will continue to offer at least a rudimentary version of every single HCAV test outlined.<br />

## Can I fork and extend this?
**Yes! Please customise this to your specific needs.**<br />
The engine's code is designed to be painless to extend and modify - with every single facet standardised and templated.

Here is a non exhaustive list of extendable objects:
 - Stimuli Classes:
   - gStimuli: graphical stimulus template with standardised functions for drawing, moving, etc...
   - aStimuli: auditory stimulus template with standardised functions for playing, pitch shifting, etc...
   - tStimuli: text stimulus template with standardised functions for drawing, updating, etc...
   - Clickable: clickable object with bounding box, connections to g/a/t stimuli with functions for triggering, hovering, etc...
   - Typeable: objects containing user keyboard interactions involving linguistic or numerical inputs.
 - Session Classes:
   - Trial: a template for a specific series of configurable events and interactions, that returns a user interaction analysis.
   - Test: a control flow of trials designed to compute user fitness from the sequence of interaction analyses.
   - Exam: a collection of tests designed to compute the HCAV metric in it's entirety - either explicitly or through interpolation.
   - TrainingScheduler: a control flow of trial's and tests designed to improve an individuals HCAV exam performance.
 - Control Classes:
   - Menu: a fully customisable class to control user exhange.
   - History: collection, management, and storage of user performance on all session instances.
   - Stats: an interactive UX control flow to analyse user history.
   - Proxy: a control flow that externalises state management for remote session management and data aquisition.
   - Export: a serialisation schema for encoding user history into portable files or uploading to cloud services.
