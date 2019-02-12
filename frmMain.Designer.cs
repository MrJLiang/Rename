namespace HAAGONtest
{
    partial class frmMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabP_AutoRun = new System.Windows.Forms.TabPage();
            this.TabP_ProdChange = new System.Windows.Forms.TabPage();
            this.imageList = new System.Windows.Forms.ImageList(this.components);
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel3 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel5 = new System.Windows.Forms.ToolStripStatusLabel();
            this.TabP_ConnectTest = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabP_AutoRun);
            this.tabControl1.Controls.Add(this.TabP_ProdChange);
            this.tabControl1.Controls.Add(this.TabP_ConnectTest);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tabControl1.ImageList = this.imageList;
            this.tabControl1.ItemSize = new System.Drawing.Size(100, 38);
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1350, 706);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabP_AutoRun
            // 
            this.tabP_AutoRun.ImageIndex = 51;
            this.tabP_AutoRun.Location = new System.Drawing.Point(4, 42);
            this.tabP_AutoRun.Name = "tabP_AutoRun";
            this.tabP_AutoRun.Padding = new System.Windows.Forms.Padding(3);
            this.tabP_AutoRun.Size = new System.Drawing.Size(1342, 660);
            this.tabP_AutoRun.TabIndex = 0;
            this.tabP_AutoRun.Text = "自动运行";
            this.tabP_AutoRun.UseVisualStyleBackColor = true;
            // 
            // TabP_ProdChange
            // 
            this.TabP_ProdChange.ImageIndex = 98;
            this.TabP_ProdChange.Location = new System.Drawing.Point(4, 42);
            this.TabP_ProdChange.Name = "TabP_ProdChange";
            this.TabP_ProdChange.Padding = new System.Windows.Forms.Padding(3);
            this.TabP_ProdChange.Size = new System.Drawing.Size(1342, 660);
            this.TabP_ProdChange.TabIndex = 1;
            this.TabP_ProdChange.Text = "产品换型";
            this.TabP_ProdChange.UseVisualStyleBackColor = true;
            // 
            // imageList
            // 
            this.imageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList.ImageStream")));
            this.imageList.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList.Images.SetKeyName(0, "application_pdf.png");
            this.imageList.Images.SetKeyName(1, "closed.png");
            this.imageList.Images.SetKeyName(2, "ic_3d_disp_center_mode.png");
            this.imageList.Images.SetKeyName(3, "ic_3d_disp_id.png");
            this.imageList.Images.SetKeyName(4, "ic_3d_disp_lines.png");
            this.imageList.Images.SetKeyName(5, "ic_3d_disp_normals.png");
            this.imageList.Images.SetKeyName(6, "ic_3d_disp_reset.png");
            this.imageList.Images.SetKeyName(7, "ic_3d_disp_surface_mode.png");
            this.imageList.Images.SetKeyName(8, "ic_3d_plot_axes.png");
            this.imageList.Images.SetKeyName(9, "ic_3d_plot_mode.png");
            this.imageList.Images.SetKeyName(10, "ic_abort_proc.png");
            this.imageList.Images.SetKeyName(11, "ic_active.png");
            this.imageList.Images.SetKeyName(12, "ic_apply_lines.png");
            this.imageList.Images.SetKeyName(13, "ic_aspect_window.png");
            this.imageList.Images.SetKeyName(14, "ic_aspect_zoom.png");
            this.imageList.Images.SetKeyName(15, "ic_back.png");
            this.imageList.Images.SetKeyName(16, "ic_bookmark.png");
            this.imageList.Images.SetKeyName(17, "ic_breakpoint.png");
            this.imageList.Images.SetKeyName(18, "ic_breakpoint_activate_all.png");
            this.imageList.Images.SetKeyName(19, "ic_breakpoint_deactivate.png");
            this.imageList.Images.SetKeyName(20, "ic_breakpoint_deactivate_all.png");
            this.imageList.Images.SetKeyName(21, "ic_call_stack.png");
            this.imageList.Images.SetKeyName(22, "ic_cascade_windows.png");
            this.imageList.Images.SetKeyName(23, "ic_clean_up.png");
            this.imageList.Images.SetKeyName(24, "ic_clear_all_bookmarks.png");
            this.imageList.Images.SetKeyName(25, "ic_clear_all_breakpoints.png");
            this.imageList.Images.SetKeyName(26, "ic_clear_bookmark.png");
            this.imageList.Images.SetKeyName(27, "ic_clear_breakpoint.png");
            this.imageList.Images.SetKeyName(28, "ic_clear_roi_all.png");
            this.imageList.Images.SetKeyName(29, "ic_clear_roi_sel.png");
            this.imageList.Images.SetKeyName(30, "ic_clear_window.png");
            this.imageList.Images.SetKeyName(31, "ic_close.png");
            this.imageList.Images.SetKeyName(32, "ic_copy.png");
            this.imageList.Images.SetKeyName(33, "ic_cut.png");
            this.imageList.Images.SetKeyName(34, "ic_deactive.png");
            this.imageList.Images.SetKeyName(35, "ic_delete.png");
            this.imageList.Images.SetKeyName(36, "ic_event_cat_exception.png");
            this.imageList.Images.SetKeyName(37, "ic_event_cat_exec_error.png");
            this.imageList.Images.SetKeyName(38, "ic_event_cat_exec_info.png");
            this.imageList.Images.SetKeyName(39, "ic_event_cat_exec_warning.png");
            this.imageList.Images.SetKeyName(40, "ic_event_cat_file_info.png");
            this.imageList.Images.SetKeyName(41, "ic_event_cat_file_warning.png");
            this.imageList.Images.SetKeyName(42, "ic_event_cat_hlib_error.png");
            this.imageList.Images.SetKeyName(43, "ic_event_cat_hlib_warning.png");
            this.imageList.Images.SetKeyName(44, "ic_event_cat_jit_info.png");
            this.imageList.Images.SetKeyName(45, "ic_event_cat_jit_warning.png");
            this.imageList.Images.SetKeyName(46, "ic_event_cat_procline_warning.png");
            this.imageList.Images.SetKeyName(47, "ic_exit.png");
            this.imageList.Images.SetKeyName(48, "ic_export.png");
            this.imageList.Images.SetKeyName(49, "ic_find.png");
            this.imageList.Images.SetKeyName(50, "ic_find_again.png");
            this.imageList.Images.SetKeyName(51, "ic_forward.png");
            this.imageList.Images.SetKeyName(52, "ic_gen_arbitrary_reg.png");
            this.imageList.Images.SetKeyName(53, "ic_gen_circle.png");
            this.imageList.Images.SetKeyName(54, "ic_gen_circular_arc.png");
            this.imageList.Images.SetKeyName(55, "ic_gen_ellipse.png");
            this.imageList.Images.SetKeyName(56, "ic_gen_elliptical_arc.png");
            this.imageList.Images.SetKeyName(57, "ic_gen_line.png");
            this.imageList.Images.SetKeyName(58, "ic_gen_nurbs.png");
            this.imageList.Images.SetKeyName(59, "ic_gen_nurbs_interp.png");
            this.imageList.Images.SetKeyName(60, "ic_gen_rect1.png");
            this.imageList.Images.SetKeyName(61, "ic_gen_rect2.png");
            this.imageList.Images.SetKeyName(62, "ic_generate_code.png");
            this.imageList.Images.SetKeyName(63, "ic_hand.png");
            this.imageList.Images.SetKeyName(64, "ic_help.png");
            this.imageList.Images.SetKeyName(65, "ic_help_locate.png");
            this.imageList.Images.SetKeyName(66, "ic_hide_ops.png");
            this.imageList.Images.SetKeyName(67, "ic_hide_procs.png");
            this.imageList.Images.SetKeyName(68, "ic_hide_uncalled.png");
            this.imageList.Images.SetKeyName(69, "ic_histo.png");
            this.imageList.Images.SetKeyName(70, "ic_histo_standard_size.png");
            this.imageList.Images.SetKeyName(71, "ic_home.png");
            this.imageList.Images.SetKeyName(72, "ic_insert_cursor.png");
            this.imageList.Images.SetKeyName(73, "ic_insert_operator.png");
            this.imageList.Images.SetKeyName(74, "ic_light_off.png");
            this.imageList.Images.SetKeyName(75, "ic_light_on.png");
            this.imageList.Images.SetKeyName(76, "ic_line_profile.png");
            this.imageList.Images.SetKeyName(77, "ic_lineal.png");
            this.imageList.Images.SetKeyName(78, "ic_linear.png");
            this.imageList.Images.SetKeyName(79, "ic_logarithmic.png");
            this.imageList.Images.SetKeyName(80, "ic_magnify-glass.png");
            this.imageList.Images.SetKeyName(81, "ic_new.png");
            this.imageList.Images.SetKeyName(82, "ic_ocr_tfb.png");
            this.imageList.Images.SetKeyName(83, "ic_online-zoom.png");
            this.imageList.Images.SetKeyName(84, "ic_open.png");
            this.imageList.Images.SetKeyName(85, "ic_open_event_log_wnd.png");
            this.imageList.Images.SetKeyName(86, "ic_open_example.png");
            this.imageList.Images.SetKeyName(87, "ic_open_graphics_wnd.png");
            this.imageList.Images.SetKeyName(88, "ic_open_new_tab.png");
            this.imageList.Images.SetKeyName(89, "ic_open_operator_wnd.png");
            this.imageList.Images.SetKeyName(90, "ic_open_prog.png");
            this.imageList.Images.SetKeyName(91, "ic_open_prog_line_view.png");
            this.imageList.Images.SetKeyName(92, "ic_open_prog_listing_wnd.png");
            this.imageList.Images.SetKeyName(93, "ic_open_start_dlg.png");
            this.imageList.Images.SetKeyName(94, "ic_open_var_view_wnd.png");
            this.imageList.Images.SetKeyName(95, "ic_organize_windows.png");
            this.imageList.Images.SetKeyName(96, "ic_paste.png");
            this.imageList.Images.SetKeyName(97, "ic_paste_insert.png");
            this.imageList.Images.SetKeyName(98, "ic_pref_procs_16.png");
            this.imageList.Images.SetKeyName(99, "ic_preferences.png");
            this.imageList.Images.SetKeyName(100, "ic_print.png");
            this.imageList.Images.SetKeyName(101, "ic_proc_lib_new.png");
            this.imageList.Images.SetKeyName(102, "ic_proc_local.png");
            this.imageList.Images.SetKeyName(103, "ic_prof_disp_time.png");
            this.imageList.Images.SetKeyName(104, "ic_prof_num_calls.png");
            this.imageList.Images.SetKeyName(105, "ic_prof_profiler.png");
            this.imageList.Images.SetKeyName(106, "ic_prof_reset.png");
            this.imageList.Images.SetKeyName(107, "ic_prof_rt_stats.png");
            this.imageList.Images.SetKeyName(108, "ic_prof_time_exec.png");
            this.imageList.Images.SetKeyName(109, "ic_prof_time_op.png");
            this.imageList.Images.SetKeyName(110, "ic_prof_time_sum.png");
            this.imageList.Images.SetKeyName(111, "ic_prog_auto_completion_off.png");
            this.imageList.Images.SetKeyName(112, "ic_prog_counter.png");
            this.imageList.Images.SetKeyName(113, "ic_prog_edit_interface.png");
            this.imageList.Images.SetKeyName(114, "ic_prog_enter_execute_off.png");
            this.imageList.Images.SetKeyName(115, "ic_prog_listing_pin_off.png");
            this.imageList.Images.SetKeyName(116, "ic_prog_mode_editor_off.png");
            this.imageList.Images.SetKeyName(117, "ic_read_image.png");
            this.imageList.Images.SetKeyName(118, "ic_redo.png");
            this.imageList.Images.SetKeyName(119, "ic_reghisto.png");
            this.imageList.Images.SetKeyName(120, "ic_region.png");
            this.imageList.Images.SetKeyName(121, "ic_reset.png");
            this.imageList.Images.SetKeyName(122, "ic_reset_horz.png");
            this.imageList.Images.SetKeyName(123, "ic_reset_horz_vert.png");
            this.imageList.Images.SetKeyName(124, "ic_reset_proc.png");
            this.imageList.Images.SetKeyName(125, "ic_reset_vert.png");
            this.imageList.Images.SetKeyName(126, "ic_roi_create.png");
            this.imageList.Images.SetKeyName(127, "ic_roi_edit_reg.png");
            this.imageList.Images.SetKeyName(128, "ic_roi_mode_path.png");
            this.imageList.Images.SetKeyName(129, "ic_roi_mode_region.png");
            this.imageList.Images.SetKeyName(130, "ic_roi_mode_xld.png");
            this.imageList.Images.SetKeyName(131, "ic_roi_op_add_roi.png");
            this.imageList.Images.SetKeyName(132, "ic_roi_op_complement.png");
            this.imageList.Images.SetKeyName(133, "ic_roi_op_complement_off.png");
            this.imageList.Images.SetKeyName(134, "ic_roi_op_difference.png");
            this.imageList.Images.SetKeyName(135, "ic_roi_op_intersection.png");
            this.imageList.Images.SetKeyName(136, "ic_roi_op_union.png");
            this.imageList.Images.SetKeyName(137, "ic_roi_op_xor.png");
            this.imageList.Images.SetKeyName(138, "ic_run.png");
            this.imageList.Images.SetKeyName(139, "ic_run_to_cursor.png");
            this.imageList.Images.SetKeyName(140, "ic_run_to_insert_cursor.png");
            this.imageList.Images.SetKeyName(141, "ic_save.png");
            this.imageList.Images.SetKeyName(142, "ic_save_as.png");
            this.imageList.Images.SetKeyName(143, "ic_save_proc_as.png");
            this.imageList.Images.SetKeyName(144, "ic_save_prog.png");
            this.imageList.Images.SetKeyName(145, "ic_save_prog_procs.png");
            this.imageList.Images.SetKeyName(146, "ic_select_arrow.png");
            this.imageList.Images.SetKeyName(147, "ic_set.png");
            this.imageList.Images.SetKeyName(148, "ic_set_sdi.png");
            this.imageList.Images.SetKeyName(149, "ic_step.png");
            this.imageList.Images.SetKeyName(150, "ic_stepforward.png");
            this.imageList.Images.SetKeyName(151, "ic_stepinto.png");
            this.imageList.Images.SetKeyName(152, "ic_stepout.png");
            this.imageList.Images.SetKeyName(153, "ic_stop.png");
            this.imageList.Images.SetKeyName(154, "ic_thread_callstack.png");
            this.imageList.Images.SetKeyName(155, "ic_undo.png");
            this.imageList.Images.SetKeyName(156, "ic_zoom_adjust_win_size.png");
            this.imageList.Images.SetKeyName(157, "ic_zoom_in.png");
            this.imageList.Images.SetKeyName(158, "ic_zoom_out.png");
            this.imageList.Images.SetKeyName(159, "ic_zoom_reset.png");
            this.imageList.Images.SetKeyName(160, "open.png");
            this.imageList.Images.SetKeyName(161, "orange-sqr-dot.png");
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripStatusLabel3,
            this.toolStripStatusLabel4,
            this.toolStripStatusLabel5});
            this.statusStrip1.Location = new System.Drawing.Point(0, 708);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1350, 22);
            this.statusStrip1.TabIndex = 2;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(92, 17);
            this.toolStripStatusLabel1.Text = "机台运行状态：";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(306, 17);
            this.toolStripStatusLabel2.Text = "运行中......                                                             ";
            // 
            // toolStripStatusLabel3
            // 
            this.toolStripStatusLabel3.Name = "toolStripStatusLabel3";
            this.toolStripStatusLabel3.Size = new System.Drawing.Size(64, 17);
            this.toolStripStatusLabel3.Text = "Version：";
            // 
            // toolStripStatusLabel4
            // 
            this.toolStripStatusLabel4.Name = "toolStripStatusLabel4";
            this.toolStripStatusLabel4.Size = new System.Drawing.Size(339, 17);
            this.toolStripStatusLabel4.Text = "1.1.00001                                                                     ";
            // 
            // toolStripStatusLabel5
            // 
            this.toolStripStatusLabel5.Name = "toolStripStatusLabel5";
            this.toolStripStatusLabel5.Size = new System.Drawing.Size(223, 17);
            this.toolStripStatusLabel5.Text = "Copyright © 2018 HAAGON 版权所有";
            // 
            // TabP_ConnectTest
            // 
            this.TabP_ConnectTest.ImageIndex = 113;
            this.TabP_ConnectTest.Location = new System.Drawing.Point(4, 42);
            this.TabP_ConnectTest.Name = "TabP_ConnectTest";
            this.TabP_ConnectTest.Padding = new System.Windows.Forms.Padding(3);
            this.TabP_ConnectTest.Size = new System.Drawing.Size(1342, 660);
            this.TabP_ConnectTest.TabIndex = 2;
            this.TabP_ConnectTest.Text = "通信测试";
            this.TabP_ConnectTest.UseVisualStyleBackColor = true;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1350, 730);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tabControl1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.MaximizeBox = false;
            this.Name = "frmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "哈工自控";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.tabControl1.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.TabPage tabP_AutoRun;
        public System.Windows.Forms.TabPage TabP_ProdChange;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel3;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel4;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel5;
        private System.Windows.Forms.ImageList imageList;
        private System.Windows.Forms.TabPage TabP_ConnectTest;
    }
}