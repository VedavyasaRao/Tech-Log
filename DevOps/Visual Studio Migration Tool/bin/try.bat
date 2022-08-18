"%ildasmpath%" %1 /metadata[=MDHEADER] /text /noil > %2
"%dumpbinpath%" /dependents %1 | find /i "msvc" > %3
