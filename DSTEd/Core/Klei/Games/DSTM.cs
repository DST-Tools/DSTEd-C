namespace DSTEd.Core.Klei.Games {
    class DSTM : KleiGame {
        public DSTM() {
            this.id = 245850;
            this.name = "Mod Tools";

            this.AddTool("FMOD_DESIGNER", "FMOD_Designer/fmod_designer.exe");
            this.AddTool("SPRITER", "Spriter/Spriter.exe");
            this.AddTool("TILED", "Tiled/tiled.exe");
            this.AddTool("TEXTURE_VIEWER", "tools/bin/TextureViewer.exe");

            /*
             * @ToDo add scripts with specific arguments/parameters (see these secripts, check their help files & arguments)
             * compiler_scripts/image_build.py
             * compiler_scripts/properties.py
             * compiler_scripts/properties.pyc
             * compiler_scripts/zipanim.py
             * 
             * mod_tools/export.py
             * mod_tools/ExportOptions.py
             * mod_tools/ResizeInfo.py
             * mod_tools/validate.py
             * 
             * scripts/ds_to_spriter.py
             * scripts/resize.py
             * 
             * tools/bin/ShaderCompiler.exe
             * tools/bin/TextureConverter.exe
             */
        }
    }
}
