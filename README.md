# Wave Collapse Test
This is a Unity project experimenting with Wave Function Collapse to generate tilemaps. The focus of this project is creating interactive gameplay using level generation. After all the most fun bit in Wave Function Collapse is not necessarily the finished product but watching the world coalesce into shape. The basic rule set is borrowed from Lanton’s Ant, where moving onto a square toggles that square’s state. This translated to wave collapse means that if the player moves onto an uncollapsed tile it collapses into a definite state and if the player moves onto a collapsed tile it gets flipped back into an uncollapsed state. To achieve only a partially collapsed map I introduced an entropy threshold, only if the entropy of a square is below the entropy threshold can it collapse. 

# Links
Orignial WFC  
https://github.com/mxgmn/WaveFunctionCollapse  
Lanton's Ant  
https://www.youtube.com/watch?v=NWBToaXK5T0&ab_channel=Numberphile  
WFC's Explanation  
https://www.youtube.com/watch?v=zIRTOgfsjl0&ab_channel=DVGen  
