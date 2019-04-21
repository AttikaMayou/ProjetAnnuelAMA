#Auteur : Margot

import maya.cmds as cmds
import maya.mel as mel
import pymel.core as pmc
from functools import partial

#-----------------------------------CREATE SHADER-----------------------------------------------------
def createShader():
    if cmds.objExists('BasicShader'):
       print('shader created')
    else: 
        #create material basic  
        myShaderBasic = cmds.shadingNode('lambert', n='BasicShader', asShader=True)
        myShaderBasicSG = cmds.sets(name='%sSG' % myShaderBasic, empty=True, renderable=True, noSurfaceShader=True)
        cmds.connectAttr('BasicShader.outColor', 'BasicShaderSG.surfaceShader')       
        textureBasic = cmds.shadingNode("file", asTexture=True, n = "TextureBasic")
        cmds.setAttr('TextureBasic.fileTextureName', fileBasic, type="string")
        cmds.shadingNode("place2dTexture",asUtility=True)
        cmds.connectAttr('place2dTexture1.outUV', 'TextureBasic.uvCoord', f=True)
        cmds.connectAttr('TextureBasic.outColor', 'BasicShader.color', f=True)
        #create material frozen
        myShaderFrozen = cmds.shadingNode('lambert', n='FrozenShader', asShader=True)
        myShaderFrozenSG = cmds.sets(name='%sSG' % myShaderFrozen, empty=True, renderable=True, noSurfaceShader=True)
        cmds.connectAttr('FrozenShader.outColor', 'FrozenShaderSG.surfaceShader')   
        textureFrozen = cmds.shadingNode("file", asTexture=True, n = "TextureFrozen")
        cmds.setAttr('TextureFrozen.fileTextureName', fileFrozen, type="string")
        cmds.shadingNode("place2dTexture",asUtility=True)
        cmds.connectAttr('place2dTexture2.outUV', 'TextureFrozen.uvCoord', f=True)
        cmds.connectAttr('TextureFrozen.outColor', 'FrozenShader.color', f=True)
        #create material desert
        myShaderDesert = cmds.shadingNode('lambert', n='DesertShader', asShader=True)
        myShaderDesertSG = cmds.sets(name='%sSG' % myShaderDesert, empty=True, renderable=True, noSurfaceShader=True)
        cmds.connectAttr('DesertShader.outColor', 'DesertShaderSG.surfaceShader')   
        textureFrozen = cmds.shadingNode("file", asTexture=True, n = "TextureDesert")
        cmds.setAttr('TextureDesert.fileTextureName', fileDesert, type="string")
        cmds.shadingNode("place2dTexture",asUtility=True)
        cmds.connectAttr('place2dTexture3.outUV', 'TextureDesert.uvCoord', f=True)
        cmds.connectAttr('TextureDesert.outColor', 'DesertShader.color', f=True)


#-----------------------------------TEXTURE PATH-------------------------------------------------------
def setTexturePath():
    basicFilter = "Image Files (*.jpg *.jpeg *.tga *.png *.tiff *.bmp *.psd)"
    global fileBasic
    global fileFrozen
    global fileDesert
    fileBasic = cmds.fileDialog2 (fileFilter=basicFilter, dialogStyle=2, fm=1)
    fileBasic = fileBasic[0]
    fileFrozen = cmds.fileDialog2 (fileFilter=basicFilter, dialogStyle=2, fm=1)
    fileDesert = cmds.fileDialog2 (fileFilter=basicFilter, dialogStyle=2, fm=1)
    fileFrozen = fileFrozen[0]
    fileDesert = fileDesert[0]
    if cmds.objExists('checkShader'):
        print('shader created')
    else:   
       createShader()
    
#-------------------------------------GET COLOR BUTTON ------------------------------------------------
def chooseUvColor(uvSet, *args):
   unfoldUv(uvSet)
  
#-------------------------------------WINDOW-----------------------------------------------------------
if cmds.window('QuickSetUp', exists=True):
	cmds.deleteUI('QuickSetUp')

cmds.window('QuickSetUp',width=180, height=200, s = False)
cmds.columnLayout(columnAttach=('both', 6), columnWidth=218)
cmds.separator(h=10)
cmds.text("Set Texture")
cmds.separator(h=10)

cmds.button(label="Set Textures Path", width=200, command = 'setTexturePath()')
cmds.button(label="Path Already Set", width=200, command = 'createShader()')

cmds.separator(h=10)
cmds.text("Apply Color")
cmds.separator(h=10)

cmds.button(label="Is New Object ?", width=205, command = 'newObject()')
cmds.separator(h=10)

biomeMenu = cmds.optionMenu( label='Biome', changeCommand = 'colorButton()')
cmds.menuItem( label='Choose Biome')
cmds.menuItem( label='Basic' )
cmds.menuItem( label='Frozen' )
cmds.menuItem( label='Desert' )
cmds.separator(h=10)
    
cmds.button(label="Apply Biome", width=205, command = 'applyShader()')
cmds.separator(h=10)

cmds.rowLayout(numberOfColumns=6, columnWidth2=(50, 50))
buv1 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 1))
buv2 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 2))
buv3 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 3))

cmds.setParent('..')
cmds.rowLayout(numberOfColumns=6, columnWidth2=(100, 100))
buv4 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 4))
buv5 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 5))
buv6 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 6))

cmds.setParent('..')
cmds.rowLayout(numberOfColumns=6, columnWidth2=(100, 100))
buv7 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 7))
buv8 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 8))
buv9 = cmds.button(label=' ', width = 65, h=65, c=partial(chooseUvColor, 9))

cmds.setParent('..')
cmds.separator(h=10)
cmds.text("Automatic UVs :")
checkUv = cmds.checkBoxGrp( numberOfCheckBoxes=2, labelArray2=['Set 1', 'Set 2'] )
cmds.separator(h=10)
cmds.button(label="Clean Scene", width=205, command = 'cleanScene()')

cmds.showWindow()

#---------------------------------------UI COLOR---------------------------------------
def colorButton():
    currentValue = cmds.optionMenu(biomeMenu, query=True, value=True)
    if currentValue == 'Basic':
        pmc.button(buv1, edit=True, backgroundColor = (0.539471, 0.715708, 0.346709))
        pmc.button(buv2, edit=True, backgroundColor = (0.386429, 0.473523, 0.205076))
        pmc.button(buv3, edit=True, backgroundColor = (0.201554, 0.34192, 0.0822816))
        pmc.button(buv4, edit=True, backgroundColor = (0.439651, 0.34192, 0.238395))
        pmc.button(buv5, edit=True, backgroundColor = (0.144131, 0.116972, 0.0864988))
        pmc.button(buv6, edit=True, backgroundColor = (0.0512699, 0.138434, 0.0630113))
        pmc.button(buv7, edit=True, backgroundColor = (0.806955, 0.806955, 0.806955))
        pmc.button(buv8, edit=True, backgroundColor = (0.651417, 0.679556, 0.715708))
        pmc.button(buv9, edit=True, backgroundColor = (0.327785, 0.356404, 0.36131))
    if currentValue == 'Frozen':
        pmc.button(buv1, edit=True, backgroundColor = (0.88791, 0.896255, 0.87961))
        pmc.button(buv2, edit=True, backgroundColor = (0.571122, 0.527105, 0.552005))
        pmc.button(buv3, edit=True, backgroundColor = (0.327785, 0.356404, 0.36131))
        pmc.button(buv4, edit=True, backgroundColor = (0.791304, 0.913082, 0.921566))
        pmc.button(buv5, edit=True, backgroundColor = (0.346709, 0.539471, 0.658387))
        pmc.button(buv6, edit=True, backgroundColor = (0.450779, 0.896255, 0.760535))
        pmc.button(buv7, edit=True, backgroundColor = (0.617213, 0.6654, 0.745418 ))
        pmc.button(buv8, edit=True, backgroundColor = (0.36131, 0.508871, 0.672457))
        pmc.button(buv9, edit=True, backgroundColor = (0.181162, 0.327785, 0.381326))
    if currentValue == 'Desert':
        pmc.button(buv1, edit=True, backgroundColor = (0.87961, 0.783544, 0.637607 ))
        pmc.button(buv2, edit=True, backgroundColor = (0.863147, 0.64449, 0.462069))
        pmc.button(buv3, edit=True, backgroundColor = (0.775831, 0.508871, 0.332458))
        pmc.button(buv4, edit=True, backgroundColor = (0.846866, 0.6654, 0.558335))
        pmc.button(buv5, edit=True, backgroundColor = (0.730474, 0.450779, 0.391571))
        pmc.button(buv6, edit=True, backgroundColor = (0.597204, 0.34192, 0.318555))
        pmc.button(buv7, edit=True, backgroundColor = (0.637607, 0.313996, 0.219522))
        pmc.button(buv8, edit=True, backgroundColor = (0.527105, 0.327785, 0.242279))
        pmc.button(buv9, edit=True, backgroundColor = (0.417881, 0.171438, 0.0822816))
   
def newObject():
    objects = cmds.ls(selection=True)
    newname = "SM_Basic"
    for item in objects:
        cmds.rename(newname + '_' + item)
        
#----------------------------------APPLY SHADER----------------------------------------
def setShader(fileBasic, fileFrozen, fileDesert):
    sel = cmds.ls(selection=True)
    currentValue = cmds.optionMenu(biomeMenu, query=True, value=True)
    if currentValue == 'Basic':
        newname = "SM_Basic_"
        cmds.hyperShade( assign='BasicShader')
    if currentValue == 'Frozen':
        newname = "SM_Frozen_"
        cmds.hyperShade( assign='FrozenShader')
    if currentValue == 'Desert':
        newname = "SM_Desert_"
        cmds.hyperShade( assign='DesertShader')
    for item in pmc.selected():
            item.rename(item.name().replace('SM_Basic_', newname))
            item.rename(item.name().replace('SM_Frozen_', newname))
            item.rename(item.name().replace('SM_Desert_', newname))
            
def applyShader():
   setShader(fileBasic, fileFrozen, fileDesert)
    
#-------------------------------------UNFOLD UVS----------------------------------------
def unfoldUv(uvSet):
   cmds.polyPlanarProjection(md = 'y')
   cmds.polyEditUV( relative=True, su=0.25, sv=0.25 )
   if uvSet is 1:
       cmds.polyEditUV( relative=True, uValue=0.05, vValue=0.7)
   if uvSet is 2 :
       cmds.polyEditUV( relative=True, uValue=0.35, vValue=0.7)
   if uvSet is 3:
       cmds.polyEditUV( relative=True, uValue=0.7, vValue=0.7) 
   if uvSet is 4:
       cmds.polyEditUV( relative=True, uValue=0.05, vValue=0.35)    
   if uvSet is 5:
       cmds.polyEditUV( relative=True, uValue=0.35, vValue=0.35)       
   if uvSet is 6 :
       cmds.polyEditUV( relative=True, uValue=0.7, vValue=0.35)
   if uvSet is 7 :
       cmds.polyEditUV( relative=True, uValue=0.05, vValue=0.05)
   if uvSet is 8:
       cmds.polyEditUV( relative=True, uValue=0.35, vValue=0.05)    
   if uvSet is 9:
       cmds.polyEditUV( relative=True, uValue=0.7, vValue=0.05)    

        
#-------------------------------------CLEAN SCENE -------------------------------------------
def cleanScene():
    boolUv1 = False
    boolUv2 = False
    #List of objects (geometry only)
    objList = cmds.ls(geometry=True)
    #List of objects transforms
    transforms = cmds.listRelatives(objList, p=True, path=True)
   
    #hard edge
    for i in objList:        
        cmds.select(i)
        cmds.polySoftEdge(a=0)
        #UV automatic
        if(cmds.checkBoxGrp(checkUv,q=1,v1=1)):
            boolUv1 = True 
        if(cmds.checkBoxGrp(checkUv,q=1,v2=1)):
            boolUv2 = True
            cmds.polyUVSet( create=True, uvSet='map2')

        if boolUv1 or boolUv2 is True:
            cmds.polyAutoProjection( i + '.f[*]' )

    #placement pivot
    for j in transforms: 
        bbox = cmds.exactWorldBoundingBox(j)
        bottom = [(bbox[0] + bbox[3])/2, bbox[1], (bbox[2] + bbox[5])/2]
        cmds.select(j)
        cmds.xform(piv = bottom, ws=True)
        #snap à zéro
        pivot = cmds.getAttr(j+".scalePivot")
        #freeze transform for new position
        cmds.makeIdentity(j, apply=True) 
        pivot = cmds.xform(j,q=1,ws=1,rp=1)      
        cmds.move(-pivot[0], -pivot[1], -pivot[2])
        #delete all by type
        cmds.delete(ch=True)
        #freeze transform in origin
        cmds.makeIdentity(apply=True) 
 